public class QuizTextParser
{
    public class QuestionData
    {
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public int CorrectAnswerIndex { get; set; }
    }

    public List<QuestionData> ParseQuestions(string quizText)
    {
        var questions = new List<QuestionData>();
        var lines = quizText.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);

        QuestionData? currentQuestion = null;
        var currentOptions = new List<string>();

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Salta le linee vuote
            if (string.IsNullOrWhiteSpace(trimmedLine))
                continue;

            // Se la linea non inizia con tab, è una nuova domanda
            if (!line.StartsWith("\t"))
            {
                // Se abbiamo una domanda precedente, la salviamo
                if (currentQuestion != null && currentOptions.Any())
                {
                    currentQuestion.Options = currentOptions.ToList();
                    questions.Add(currentQuestion);
                }

                // Inizia una nuova domanda
                currentQuestion = new QuestionData { Text = trimmedLine };
                currentOptions = new List<string>();
            }
            // Se la linea inizia con tab, è un'opzione
            else if (currentQuestion != null)
            {
                var optionText = trimmedLine;

                // Controlla se è la risposta corretta
                if (optionText.EndsWith("✔️"))
                {
                    currentQuestion.CorrectAnswerIndex = currentOptions.Count;
                    optionText = optionText.Replace("✔️", "").Trim();
                }

                currentOptions.Add(optionText);
            }
        }

        // Aggiungi l'ultima domanda se presente
        if (currentQuestion != null && currentOptions.Any())
        {
            currentQuestion.Options = currentOptions;
            questions.Add(currentQuestion);
        }

        return questions;
    }
}
