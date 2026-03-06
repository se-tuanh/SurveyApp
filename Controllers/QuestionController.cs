using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;
using SurveyApp.Models;

namespace SurveyApp.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Question/Create?surveyId=5
        public async Task<IActionResult> Create(int surveyId)
        {
            var survey = await _context.Surveys.FindAsync(surveyId);
            if (survey == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && survey.CreatedById != userId) return Forbid();

            ViewBag.SurveyId = surveyId;
            ViewBag.SurveyTitle = survey.Title;
            return View(new Question { SurveyId = surveyId });
        }

        // POST: Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question model, List<string>? optionTexts)
        {
            var survey = await _context.Surveys.FindAsync(model.SurveyId);
            if (survey == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && survey.CreatedById != userId) return Forbid();

            if (!ModelState.IsValid)
            {
                ViewBag.SurveyId = model.SurveyId;
                ViewBag.SurveyTitle = survey.Title;
                return View(model);
            }

            model.Order = await _context.Questions.CountAsync(q => q.SurveyId == model.SurveyId);
            _context.Questions.Add(model);
            await _context.SaveChangesAsync();

            // Add options if MultipleChoice
            if (model.QuestionType == QuestionType.MultipleChoice && optionTexts != null)
            {
                foreach (var text in optionTexts.Where(t => !string.IsNullOrWhiteSpace(t)))
                {
                    _context.Options.Add(new Option { QuestionId = model.Id, Text = text.Trim() });
                }
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Thêm câu hỏi thành công!";
            return RedirectToAction("Details", "Survey", new { id = model.SurveyId });
        }

        // GET: Question/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Options)
                .Include(q => q.Survey)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && question.Survey!.CreatedById != userId) return Forbid();

            return View(question);
        }

        // POST: Question/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Question model, List<string>? optionTexts)
        {
            var question = await _context.Questions.Include(q => q.Options).Include(q => q.Survey)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (question == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && question.Survey!.CreatedById != userId) return Forbid();

            question.Text = model.Text;
            question.QuestionType = model.QuestionType;

            // Clear and re-add options
            if (question.QuestionType == QuestionType.MultipleChoice)
            {
                _context.Options.RemoveRange(question.Options);
                if (optionTexts != null)
                {
                    foreach (var text in optionTexts.Where(t => !string.IsNullOrWhiteSpace(t)))
                        _context.Options.Add(new Option { QuestionId = question.Id, Text = text.Trim() });
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật câu hỏi thành công!";
            return RedirectToAction("Details", "Survey", new { id = question.SurveyId });
        }

        // POST: Question/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _context.Questions.Include(q => q.Survey).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && question.Survey!.CreatedById != userId) return Forbid();

            int surveyId = question.SurveyId;
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa câu hỏi thành công!";
            return RedirectToAction("Details", "Survey", new { id = surveyId });
        }
    }
}
