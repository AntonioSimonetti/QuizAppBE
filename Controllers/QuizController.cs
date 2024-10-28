using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuizApp.Data.DTO;
using QuizApp.Interfaces;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // Quiz operations

        [HttpPost("CreateQuiz")]

        public async Task<IActionResult> CreateQuiz([FromBody] QuizDto quizDto)
        {
            if (quizDto == null)
            {
                return BadRequest("Quiz data is required.");
            }

            var quiz = new Quiz
            {
                Title = quizDto.Title,
                IsPublic = quizDto.IsPublic,
                TimeLimit = TimeSpan.FromMinutes(quizDto.Timelimit),
                UserId = quizDto.UserId,

            };

            try
            {
                await _quizService.CreateQuizAsync(quiz);

                return Ok(quiz);
                //return CreatedAtAction(nameof(GetQuizById), new { id = quiz.Id }, quizDto);
            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteQuizById/{quizId}")]
        public async Task<IActionResult> DeleteQuiz(int quizId)
        {
            try
            {
                await _quizService.DeleteQuizAsync(quizId);
                return Ok("Quiz successfully deleted.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetQuizById/{id}")]
        public async Task<IActionResult> GetQuizById(int id)
        {
            try
            {
                var quiz = await _quizService.GetQuizByIdAsync(id);
                return Ok(quiz); // Restituisce il quiz trovato.
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Restituisce 404 se il quiz non viene trovato.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateQuiz/{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] QuizDto quizDto)
        {
            if (id != quizDto.Id)
            {
                return BadRequest("Mismatch between route ID and quiz ID in the body.");
            }

            try
            {
                var quiz = new Quiz
                {
                    Id = quizDto.Id,
                    Title = quizDto.Title,
                    IsPublic = quizDto.IsPublic,
                    TimeLimit = TimeSpan.FromMinutes(quizDto.Timelimit),
                    UserId = quizDto.UserId
                };

                await _quizService.UpdateQuizAsync(id, quiz);
                return Ok("Quiz updated successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GetAllQuizzes

        // Question operations

        [HttpPost("CreateQuestion")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionDto questionDto)
        {
            if (string.IsNullOrWhiteSpace(questionDto.Text))
            {
                return BadRequest("Question text is required.");
            }

            // Creare l'entità Question dal DTO
            var question = new Question
            {
                Text = questionDto.Text,
                CorrectAnswerIndex = questionDto.CorrectAnswerIndex,
                Points = questionDto.Points,
                NegativePoints = questionDto.NegativePoints,
                UserId = questionDto.UserId // Associare l'utente alla domanda
            };

            try
            {
                // Creare la domanda usando il servizio
                await _quizService.CreateQuestionAsync(question);

                return Ok("Question successfully created.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteQuestion/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            try
            {
                await _quizService.DeleteQuestionAsync(questionId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddQuestionToQuiz")]
        public async Task<IActionResult> AddQuestionToQuiz(int quizId, [FromBody] int questionId)
        {
            try
            {
                // Chiama il metodo del servizio per aggiungere la domanda esistente (già salvata) al quiz
                await _quizService.AddQuestionToQuizAsync(quizId, questionId);

                return Ok("Question successfully added to quiz.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{quizId}/RemoveQuestion/{questionId}")]
        public async Task<IActionResult> RemoveQuestionFromQuiz(int quizId, int questionId)
        {
            try
            {
                await _quizService.RemoveQuestionFromQuizAsync(quizId, questionId);
                return Ok("Question removed from quiz successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetQuestionsByQuizId/{quizId}")]
        public async Task<IActionResult> GetQuestionsByQuizId(int quizId)
        {
            try
            {
                var questions = await _quizService.GetQuestionsByQuizIdAsync(quizId);

                if (questions == null || !questions.Any())
                {
                    return NotFound("No questions found for this quiz.");
                }

                return Ok(questions);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateQuestion/{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] QuestionDto questionDto)
        {
            var question = new Question
            {
                Id = id,
                Text = questionDto.Text,
                CorrectAnswerIndex = questionDto.CorrectAnswerIndex,
                Points = questionDto.Points,
                NegativePoints = questionDto.NegativePoints,
                UserId = questionDto.UserId
            };
            try
            {
                await _quizService.UpdateQuestionAsync(id, question);
                return Ok("Question updated successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        // Option operations

        [HttpPost("CreateOption")]
        public async Task<IActionResult> CreateOption([FromBody] OptionDto optionDto)
        {
            if (string.IsNullOrWhiteSpace(optionDto.Text))
            {
                return BadRequest("Option text is required");
            }

            try
            {
                var option = await _quizService.CreateOptionAsync(new Option { Text = optionDto.Text });
                return Ok(option);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddOptionToQuestion")]
        public async Task<IActionResult> AddOptionToQuestion(int questionId, [FromBody] int optionId)
        {

            try
            {
                await _quizService.AddOptionToQuestionAsync(questionId, optionId);

                return Ok("Option successfully added to question.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Gestione di errori come "domanda non trovata"
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("RemoveOption/{optionId}")]

        public async Task<IActionResult> RemoveOptionFromQuiz(int optionId)
        {
            try
            {
                await _quizService.RemoveOptionFromQuestionAsync(optionId);
                return Ok("Option removed from the question successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateOption/{id}")]

        public async Task<IActionResult> UpdateOption(int id, [FromBody] OptionDto optionDto)
        {
            var option = new Option
            {
                Id = id,
                Text = optionDto.Text,
                QuestionId = optionDto.QuestionId

            };
            try
            {
                await _quizService.UpdateOptionAsync(id, option);
                return Ok("Option updated successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteOption/{optionId}")]

        public async Task<IActionResult> DeleteOption(int optionId)
        {
            try
            {
                await _quizService.DeleteOptionAsync(optionId);
                return Ok("Option deleted successfully");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetOptionsByQuestionId/{questionId}")]

        public async Task<IActionResult> GetOptionsByQuestionId(int questionId)
        {
            try
            {
                var options = await _quizService.GetOptionsByQuestionIdAsync(questionId);
                return Ok(options);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Result operations

        [HttpPost("SubmitQuizResult")]
        public async Task<IActionResult> SubmitQuizResult(int quizId, string userId, int score)
        {
            try
            {
                await _quizService.SubmitQuizResultAsync(quizId, userId, score);
                return Ok("Quiz result submitted successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("GetResultsByQuizAndUser")]

        public async Task<IActionResult> GetResultsByQuizIdAndUserId([FromBody] QuizResultDto quizResultDto)
        {
            try
            {
                var results = await _quizService.GetResultsByQuizIdAndUserIdAsync(quizResultDto.QuizId, quizResultDto.UserId);

                var resultDtos = results.Select(r => new QuizResultDto
                {
                    Id = r.Id,
                    QuizId = r.QuizId,
                    UserId = r.UserId,
                    Score = r.Score,
                    CompletedAt = r.CompletedAt
                });

                return Ok(resultDtos);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
     

    }
}
