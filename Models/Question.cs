using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class Question
    {
        public Question()
        {
            Options = new List<Option>();
            QuizQuestions = new List<QuizQuestion>();

            Text = string.Empty;

            UserId = string.Empty;

        }
        public int Id { get; set; }
        public string Text { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public int Points { get; set; }
        public int NegativePoints { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser? User { get; set; }


        public ICollection<Option> Options { get; set; }
        public ICollection<QuizQuestion> QuizQuestions { get; set; }

    }
}
