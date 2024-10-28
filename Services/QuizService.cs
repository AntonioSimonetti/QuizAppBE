using QuizApp.Data;
using System.Threading.Tasks;
using QuizApp.Interfaces;
using QuizApp.Models;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data.DTO;


namespace QuizApp.Services
{
    public class QuizService : IQuizService
    {
        private readonly AppDbContext _context;
        public QuizService(AppDbContext context) {
            _context = context;
        }

        // Quiz services
        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            var Quizzes = await _context.Quizzes
                 .Include(q => q.QuizQuestions)
                 .ThenInclude(qq => qq.Question)
                 .FirstOrDefaultAsync(q => q.Id == id);

            if (Quizzes == null) throw new ArgumentException("Quiz not found");

            return Quizzes;
        }

        public async Task<IEnumerable<Quiz>> GetAllQuizzesAsync()
        {
            // Restituisce una lista di quiz; se non ci sono quiz, restituisce una lista vuota per evitare NullReferenceException.
            // La gestione del caso in cui la lista è vuota può essere fatta nel controller o direttamente nel front-end.

            return await _context.Quizzes.ToListAsync();
        }

        public async Task CreateQuizAsync(Quiz quiz)
        {
            // Validazione
            if (string.IsNullOrWhiteSpace(quiz.Title))
            {
                throw new ArgumentException("Title must exist");
            }

            if (string.IsNullOrWhiteSpace(quiz.UserId))
            {
                throw new ArgumentException("User ID is mandatory");
            }

            // Aggiungi il quiz al contesto e salva le modifiche
            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuizAsync(int id, Quiz quiz)
        {
            // Cerca il quiz esistente nel database
            var existingQuiz = await _context.Quizzes.FindAsync(id);

            if (existingQuiz == null)
            {
                throw new ArgumentException("Quiz not found");
            }

            // Aggiorna solo i campi diretti
            existingQuiz.Title = quiz.Title;
            existingQuiz.IsPublic = quiz.IsPublic;
            existingQuiz.TimeLimit = quiz.TimeLimit;
            existingQuiz.UserId = quiz.UserId;

            // Salva i cambiamenti
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuizAsync(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                throw new ArgumentException("Quiz not found");
            }

            // Rimouvo le domande associate al quiz prima per evitare di avere dati orfani nel DB

            _context.QuizQuestions.RemoveRange(quiz.QuizQuestions);

            // Rimuovo infine il quiz
            _context.Quizzes.Remove(quiz);

            await _context.SaveChangesAsync();

        }

        // Question services

        public async Task<Question> CreateQuestionAsync(Question question)
        {
            if (string.IsNullOrWhiteSpace(question.Text))
            {
                throw new ArgumentException("Question text must not be empty");
            }

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            return question;
        }

        public async Task AddQuestionToQuizAsync(int quizId, int questionId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.QuizQuestions)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
            {
                throw new ArgumentException("Quiz not found");
            }
            /*

            if(question.Id == 0)
            {
                await _context.Questions.AddAsync(question);
                await _context.SaveChangesAsync();
            }

            quiz.QuizQuestions.Add(new QuizQuestion
            {
                QuizId = quizId,
                QuestionId = question.Id,
                Question = question
            });*/

            var question = await _context.Questions.FindAsync(questionId);

            if (question == null)
            {
                throw new ArgumentException("Question not found");
            }

            quiz.QuizQuestions.Add(new QuizQuestion
            {
                QuizId = quizId,
                QuestionId = questionId,
                Question = question
            });


            await _context.SaveChangesAsync();
        }

        public async Task RemoveQuestionFromQuizAsync(int quizId, int questionId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null)
            {
                throw new ArgumentException("Quiz not found");
            }

            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                throw new ArgumentException("Question not found");
            }

            var quizQuestion = await _context.QuizQuestions
                .FirstOrDefaultAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);

            if (quizQuestion == null)
            {
                throw new ArgumentException("The question is not associated with this quiz");
            }

            _context.QuizQuestions.Remove(quizQuestion);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuestionAsync(int id, Question question)
        {
            var existingQuestion = await _context.Questions.FindAsync(id);

            if(existingQuestion == null)
            {
                throw new ArgumentException("Question not found");
            }
            
            existingQuestion.Text = question.Text;
            existingQuestion.CorrectAnswerIndex = question.CorrectAnswerIndex;
            existingQuestion.Points = question.Points;
            existingQuestion.NegativePoints = question.NegativePoints;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(int questionId)
        {
            // Trova la domanda da eliminare
            var question = await _context.Questions.FindAsync(questionId);

            // Verifica se la domanda esiste
            if (question == null)
            {
                throw new ArgumentException("Question not found");
            }

            // Rimuovi la domanda. Grazie alla configurazione di cancellazione a cascata,
            // i riferimenti nella tabella QuizQuestions saranno gestiti automaticamente dal DB (e quindi rimossi anche da ogni singolo quiz).
            _context.Questions.Remove(question);

            // Salva le modifiche nel database
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Question?>> GetQuestionsByQuizIdAsync(int quizId)
        {
            var quiz = await GetQuizByIdAsync(quizId);

            if (quiz == null)
            {
                throw new ArgumentException("Quiz not found");
            }

            return quiz.QuizQuestions.Select(qq => qq.Question);
        }

        // Option services

        public async Task<Option> CreateOptionAsync(Option option)
        {
            if(string.IsNullOrWhiteSpace(option.Text))
            {
                throw new ArgumentException("Option text mus not be empty");
            }

            await _context.Options.AddAsync(option);
            await _context.SaveChangesAsync();

            return option;
        }

        public async Task AddOptionToQuestionAsync(int questionId, int optionId)
        {
            var question = await _context.Questions
                .Include(q => q.Options)
                .FirstOrDefaultAsync (q => q.Id == questionId);

            if(question == null)
            {
                throw new ArgumentException("Question not found");
            }

            var option = await _context.Options.FindAsync(optionId);

            if (option == null)
            {
                throw new ArgumentException("Option not found");
            }

            if(option.QuestionId != null)
            {
                throw new ArgumentException("This option is already associated with another question.");
            }
            
            option.QuestionId= questionId;
            question.Options.Add(option);
            await _context.SaveChangesAsync();

        }

        public async Task RemoveOptionFromQuestionAsync(int optionId)
        {
            var option = await _context.Options.FindAsync(optionId);

            if (option == null)
            {
                throw new ArgumentException("Option not found");
            }

            option.QuestionId = null;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateOptionAsync(int id, Option option)
        {
            var existingOption = await _context.Options.FindAsync(id);

            if(existingOption == null)
            {
                throw new ArgumentException("No option found");
            }

            if (string.IsNullOrWhiteSpace(option.Text))
            {
                throw new ArgumentException("Option text cannot be null or empty");
            }

            existingOption.Text = option.Text;

            await _context.SaveChangesAsync();

        }

        public async Task DeleteOptionAsync(int optionId)
        {
            var option = await _context.Options.FindAsync(optionId);

            if(option == null)
            {
                throw new ArgumentException("Option not found");
            }

             _context.Options.Remove(option);           

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Option>> GetOptionsByQuestionIdAsync(int questionId)
        {
            var question = await _context.Questions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if(question == null)
            {
                throw new ArgumentException("Question not found");
            }

            return question.Options;
        }

        // Result services
        public async Task SubmitQuizResultAsync(int quizId, string userId, int score)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if(quiz == null)
            {
                throw new ArgumentException("Quiz not found");
            }

            var user = await _context.Users.FindAsync(userId);

            if (quiz == null)
            {
                throw new ArgumentException("User not found");
            }

            if (score < 0)
            {
                throw new ArgumentException("Score cannot be negative.");
            }

            var quizResult = new QuizResult
            {
                QuizId = quizId,
                UserId = userId,
                Score = score,
                CompletedAt = DateTime.Now
            };

            // Da decidere se ci servono validazioni tipo, risultato non negativo etc.

            await _context.QuizResults.AddAsync(quizResult);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<QuizResult>> GetResultsByQuizIdAndUserIdAsync(int quizId, string userId)
        {
            var quizExists = await _context.Quizzes.AnyAsync(q => q.Id == quizId);
            if(!quizExists)
            {
                throw new ArgumentException("Quiz not found");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);   

            if(!userExists)
            {
                throw new ArgumentException("User not found");
            }

            var results = await _context.QuizResults
                .Where(r => r.QuizId == quizId && r.UserId == userId)
                .ToListAsync();
           
            if(!results.Any())
            {
                throw new ArgumentException("No results found for the specified quiz and user");
            }

            return results;
        }
        public async Task<IEnumerable<QuizResult>> GetResultsByQuizIdAsync(int quizId)
        {
            var results = await _context.QuizResults
                .Where(q => q.QuizId == quizId)
                .ToListAsync();

            if(!results.Any())
            {
                throw new ArgumentException("Results not found");
            }

            return results;
        }  // Not implemented in the controller yet
        public async Task<IEnumerable<QuizResult>> GetResultsByUserIdAsync(string userId)
        {
            var results = await _context.QuizResults
                .Where (q => q.UserId == userId)
                .ToListAsync();

            if (!results.Any())
            {
                throw new ArgumentException("No results found for the given user");
            }

            return results;
        } // Not implemented in the controller yet
    }
}
