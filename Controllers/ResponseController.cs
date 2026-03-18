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

            if (survey.ClosedAt.HasValue && survey.ClosedAt.Value <= DateTime.Now)
            {
                TempData["Warning"] = $"Khảo sát này đã đóng lúc {survey.ClosedAt.Value:dd/MM/yyyy HH:mm}.";
                return RedirectToAction("Index", "Home");
            }

            return View(survey);
        }

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

            if (survey.ClosedAt.HasValue && survey.ClosedAt.Value <= DateTime.Now)
            {
                TempData["Warning"] = $"Khảo sát này đã đóng lúc {survey.ClosedAt.Value:dd/MM/yyyy HH:mm}. Cập nhật không được chấp nhận.";
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
            return RedirectToAction(nameof(History));
        }

        public async Task<IActionResult> History(string filter = "all")
        {
            var userId = _userManager.GetUserId(User);

            var query = _context.Responses
                .Include(r => r.Survey)
                .ThenInclude(s => s.Questions)
                .Include(r => r.Survey.CreatedBy)
                .Where(r => r.UserId == userId)
                .AsQueryable();

            if (filter == "open")
            {
                query = query.Where(r => r.Survey.IsActive && (!r.Survey.ClosedAt.HasValue || r.Survey.ClosedAt.Value > DateTime.Now));
            }
            else if (filter == "closed")
            {
                query = query.Where(r => !r.Survey.IsActive || (r.Survey.ClosedAt.HasValue && r.Survey.ClosedAt.Value <= DateTime.Now));
            }

            var responses = await query
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();

            var viewModels = responses.Select(r => new SurveyApp.ViewModels.HistoryViewModel
            {
                ResponseId = r.Id,
                SurveyId = r.SurveyId,
                SurveyTitle = r.Survey.Title,
                CreatorName = r.Survey.CreatedBy?.FullName ?? r.Survey.CreatedBy?.Email ?? "N/A",
                SubmittedAt = r.SubmittedAt,
                ClosedAt = r.Survey.ClosedAt,
                IsActive = r.Survey.IsActive,
                TotalQuestions = r.Survey.Questions.Count
            }).ToList();

            ViewBag.CurrentFilter = filter;
            return View(viewModels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            var response = await _context.Responses
                .Include(r => r.Survey)
                    .ThenInclude(s => s.Questions)
                        .ThenInclude(q => q.Options)
                .Include(r => r.Answers)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (response == null) return NotFound();

            var viewModel = new SurveyApp.ViewModels.ResponseDetailViewModel
            {
                ResponseId = response.Id,
                Survey = response.Survey,
                UserAnswers = response.Answers.ToList(),
                SubmittedAt = response.SubmittedAt
            };

            return View(viewModel);
        }
    }
}
