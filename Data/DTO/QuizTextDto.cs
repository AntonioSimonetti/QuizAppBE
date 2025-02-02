namespace QuizApp.Data.DTO
{
    public class QuizTextDto
    {
        public string Title { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public int TimeLimit { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string QuizText { get; set; } = string.Empty;
    }
}
