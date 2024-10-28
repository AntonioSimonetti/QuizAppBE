using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class Quiz
    {
        public Quiz()
        {
            QuizQuestions = new List<QuizQuestion>();

            Title = string.Empty;

            UserId = string.Empty;  

        }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsPublic { get; set; }
        public TimeSpan? TimeLimit { get; set; }

     
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; } // Oggetto Utente Completo

        //Collezione di domande del quiz, relazione uno-a-molti
        public ICollection<QuizQuestion> QuizQuestions { get; set; }
    }
}
