using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;
using SurveyApp.Models;

namespace SurveyApp.Controllers
{
    [Authorize]
    public class ResponseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResponseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Response/Take/5
        public async Task<IActionResult> Take(int surveyId)
        {
            var survey = await _context.Surveys
                .Include(s => s.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(s => s.Id == surveyId && s.IsActive);

            if (survey == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            bool alreadyAnswered = await _context.Responses
                .AnyAsync(r => r.SurveyId == surveyId && r.UserId == userId);

            if (alreadyAnswered)
            {
                TempData["Warning"] = "Bạn đã trả lời khảo sát này rồi.";
                return RedirectToAction("Index", "Home");
            }

            return View(survey);
        }

        // POST: Response/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int surveyId, IFormCollection form)
        {
            var survey = await _context.Surveys
                .Include(s => s.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(s => s.Id == surveyId && s.IsActive);

            if (survey == null) return NotFound();

            var userId = _userManager.GetUserId(User)!;

            bool alreadyAnswered = await _context.Responses
                .AnyAsync(r => r.SurveyId == surveyId && r.UserId == userId);
            if (alreadyAnswered)
            {
                TempData["Warning"] = "Bạn đã trả lời khảo sát này rồi.";
                return RedirectToAction("Index", "Home");
            }

            var response = new Response
            {
                SurveyId = surveyId,
                UserId = userId,
                SubmittedAt = DateTime.UtcNow
            };
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();

            foreach (var question in survey.Questions)
            {
                var answer = new Answer
                {
                    ResponseId = response.Id,
                    QuestionId = question.Id
                };

                if (question.QuestionType == QuestionType.MultipleChoice)
                {
                    var selectedValue = form[$"question_{question.Id}"].FirstOrDefault();
                    if (int.TryParse(selectedValue, out int optionId))
                        answer.SelectedOptionId = optionId;
                }
                else
                {
                    answer.TextAnswer = form[$"question_{question.Id}"].FirstOrDefault() ?? string.Empty;
                }

                _context.Answers.Add(answer);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cảm ơn bạn đã hoàn thành khảo sát!";
            return RedirectToAction("Index", "Home");
        }
    }
}
