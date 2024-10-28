using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class QuizResult
    {
        public QuizResult()
        {
            UserId = string.Empty;
        }
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int Score { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
