using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;
using SurveyApp.Models;

namespace SurveyApp.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatisticsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Statistics/Index/5
        public async Task<IActionResult> Index(int surveyId)
        {
            var survey = await _context.Surveys
                .Include(s => s.CreatedBy)
                .Include(s => s.Questions).ThenInclude(q => q.Options)
                .Include(s => s.Responses).ThenInclude(r => r.Answers).ThenInclude(a => a.SelectedOption)
                .FirstOrDefaultAsync(s => s.Id == surveyId);

            if (survey == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && survey.CreatedById != userId) return Forbid();

            return View(survey);
        }
    }
}
