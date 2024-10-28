namespace QuizApp.Data.DTO
{
    public class QuizResultDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }

        public DateTime CompletedAt { get; set; }
    }
}
