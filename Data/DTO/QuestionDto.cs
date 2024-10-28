namespace QuizApp.Data.DTO
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }   
        public int CorrectAnswerIndex { get; set; }
        public int Points { get; set; }
        public int NegativePoints { get; set; }
        public string UserId { get; set; }
    }
}
