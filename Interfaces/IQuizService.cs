using QuizApp.Models;
using QuizApp.Data.DTO;

namespace QuizApp.Interfaces
{
    public interface IQuizService
    {
        // CRUD per i Quiz
        Task<Quiz> GetQuizByIdAsync(int id);
        Task<IEnumerable<Quiz>> GetAllQuizzesAsync();
        Task CreateQuizAsync(Quiz quiz);
        Task UpdateQuizAsync(int id, Quiz quiz);
        Task DeleteQuizAsync(int id);

        // Gestione delle Domande
        Task AddQuestionToQuizAsync(int quizId, int questionId);
        Task RemoveQuestionFromQuizAsync(int quizId, int questionId);
        Task<Question> CreateQuestionAsync(Question question);
        Task UpdateQuestionAsync(int id, Question question);
        Task DeleteQuestionAsync(int questionId);
        Task<IEnumerable<Question?>> GetQuestionsByQuizIdAsync(int quizId);

        // Gestione delle Options
        Task<Option> CreateOptionAsync (Option option);
        Task AddOptionToQuestionAsync(int questionId, int optionId);
        Task RemoveOptionFromQuestionAsync(int optionId);
        Task UpdateOptionAsync(int id, Option option);
        Task DeleteOptionAsync(int optionId);
        Task<IEnumerable<Option>> GetOptionsByQuestionIdAsync(int questionId);

        // Gestione Risultati
        Task SubmitQuizResultAsync(int quizId, string userId, int score);
        Task<IEnumerable<QuizResult>> GetResultsByQuizIdAndUserIdAsync(int quizId, string userId);
        Task<IEnumerable<QuizResult>> GetResultsByQuizIdAsync(int quizId);
        Task<IEnumerable<QuizResult>> GetResultsByUserIdAsync(string userId);
        
    }
}
