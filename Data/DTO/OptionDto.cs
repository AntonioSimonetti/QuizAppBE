﻿namespace QuizApp.Data.DTO
{
    public class OptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? QuestionId { get; set; }

    }
}