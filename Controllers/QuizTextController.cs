using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuizApp.Services;
using QuizApp.Models;
using QuizApp.Data.DTO;
using QuizApp.Interfaces;

namespace QuizApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizTextController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly QuizTextParser _quizParser;

        public QuizTextController(IQuizService quizService, QuizTextParser quizParser)
        {
            _quizService = quizService;
            _quizParser = quizParser;
        }

        private string FormatQuizText(string rawText)
{
    // Split into lines and remove empty ones
    var lines = rawText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    
    var formattedLines = new List<string>();
    
    foreach (var line in lines)
    {
        // Skip multiple empty lines
        if (string.IsNullOrWhiteSpace(line)) 
            continue;
            
        // If line starts with spaces/tabs and contains content, ensure it starts with exactly one tab
        if (line.TrimStart().Length > 0 && char.IsWhiteSpace(line[0]))
        {
            formattedLines.Add($"\t{line.Trim()}");
        }
        // Non-empty lines that don't start with whitespace are questions
        else if (line.Trim().Length > 0)
        {
            formattedLines.Add(line.Trim());
        }
    }
    
    // Join all lines with \n
    return string.Join("\n", formattedLines);
}

        [HttpPost("CreateFromText")]
        public async Task<IActionResult> CreateQuizFromText([FromBody] QuizTextDto quizTextDto)
        {
            try
            {
                var quiz = new Quiz
                {
                    Title = quizTextDto.Title,
                    IsPublic = quizTextDto.IsPublic,
                    TimeLimit = TimeSpan.FromMinutes(quizTextDto.TimeLimit),
                    UserId = quizTextDto.UserId
                };

                await _quizService.CreateQuizAsync(quiz);

                var questions = _quizParser.ParseQuestions(quizTextDto.QuizText);

                foreach (var questionData in questions)
                {
                    var question = new Question
                    {
                        Text = questionData.Text,
                        CorrectAnswerIndex = questionData.CorrectAnswerIndex,
                        Points = 1,
                        NegativePoints = 0,
                        UserId = quizTextDto.UserId
                    };

                    await _quizService.CreateQuestionAsync(question);
                    await _quizService.AddQuestionToQuizAsync(quiz.Id, question.Id);

                    foreach (var optionText in questionData.Options)
                    {
                        var option = new Option { Text = optionText };
                        await _quizService.CreateOptionAsync(option);
                        await _quizService.AddOptionToQuestionAsync(question.Id, option.Id);
                    }
                }

                return Ok(new { QuizId = quiz.Id, Message = "Quiz created successfully from text" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
