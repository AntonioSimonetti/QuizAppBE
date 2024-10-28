using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
