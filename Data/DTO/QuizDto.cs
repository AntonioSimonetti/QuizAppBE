namespace QuizApp.Data.DTO
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsPublic { get; set; }
        public int Timelimit { get; set; }
        public string UserId { get; set; }
    }
}

