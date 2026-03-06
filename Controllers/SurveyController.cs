using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;
using SurveyApp.Models;

namespace SurveyApp.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SurveyController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Survey
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var surveys = isAdmin
                ? await _context.Surveys.Include(s => s.CreatedBy).Include(s => s.Questions).ToListAsync()
                : await _context.Surveys.Include(s => s.CreatedBy).Include(s => s.Questions)
                    .Where(s => s.CreatedById == userId).ToListAsync();

            return View(surveys);
        }

        // GET: Survey/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var survey = await _context.Surveys
                .Include(s => s.CreatedBy)
                .Include(s => s.Questions).ThenInclude(q => q.Options)
                .Include(s => s.Responses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && survey.CreatedById != userId)
                return Forbid();

            return View(survey);
        }

        // GET: Survey/Create
        public IActionResult Create() => View();

        // POST: Survey/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Survey model)
        {
            if (!ModelState.IsValid) return View(model);

            model.CreatedById = _userManager.GetUserId(User)!;
            model.CreatedAt = DateTime.UtcNow;
            _context.Surveys.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Tạo khảo sát thành công!";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        // GET: Survey/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && survey.CreatedById != userId) return Forbid();

            return View(survey);
        }

        // POST: Survey/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Survey model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && survey.CreatedById != userId) return Forbid();

            survey.Title = model.Title;
            survey.Description = model.Description;
            survey.IsActive = model.IsActive;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật khảo sát thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Survey/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var survey = await _context.Surveys.Include(s => s.CreatedBy).FirstOrDefaultAsync(s => s.Id == id);
            if (survey == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && survey.CreatedById != userId) return Forbid();

            return View(survey);
        }

        // POST: Survey/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && survey.CreatedById != userId) return Forbid();

            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa khảo sát thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
