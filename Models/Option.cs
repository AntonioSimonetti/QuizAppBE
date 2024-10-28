using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class Option
    {
        // Inizializzare vuoto se sempre richiesto, nullable se opzionale.
        public Option()
        {
            Text = string.Empty;
            Question = null;
        }
        public int Id { get; set; }
        public string Text { get; set; }

        [ForeignKey("Question")]
        public int? QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
