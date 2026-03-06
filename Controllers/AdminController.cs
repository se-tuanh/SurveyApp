using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;
using SurveyApp.Models;
using SurveyApp.ViewModels;

namespace SurveyApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ─── DASHBOARD ────────────────────────────────────────────────────────
        public async Task<IActionResult> Dashboard()
        {
            var surveys = await _context.Surveys
                .Include(s => s.Questions)
                .Include(s => s.Responses)
                .ToListAsync();

            var recentResponses = await _context.Responses
                .Include(r => r.Survey)
                .Include(r => r.User)
                .OrderByDescending(r => r.SubmittedAt)
                .Take(8)
                .ToListAsync();

            var vm = new DashboardViewModel
            {
                TotalSurveys   = surveys.Count,
                ActiveSurveys  = surveys.Count(s => s.IsActive),
                TotalResponses = surveys.Sum(s => s.Responses.Count),
                TotalUsers     = await _userManager.Users.CountAsync(),
                TopSurveys = surveys
                    .OrderByDescending(s => s.Responses.Count)
                    .Take(5)
                    .Select(s => new SurveyStat
                    {
                        SurveyId      = s.Id,
                        Title         = s.Title,
                        ResponseCount = s.Responses.Count,
                        QuestionCount = s.Questions.Count,
                        IsActive      = s.IsActive
                    }).ToList(),
                RecentResponses = recentResponses.Select(r => new RecentResponse
                {
                    SurveyTitle = r.Survey?.Title ?? "",
                    UserName    = r.User?.FullName ?? r.User?.Email ?? "",
                    SubmittedAt = r.SubmittedAt
                }).ToList()
            };

            return View(vm);
        }

        // ─── USER MANAGEMENT ──────────────────────────────────────────────────
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users
                .Include(u => u.Surveys)
                .Include(u => u.Responses)
                .ToListAsync();

            var vmList = new List<UserManagementViewModel>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                vmList.Add(new UserManagementViewModel
                {
                    Id            = u.Id,
                    FullName      = u.FullName,
                    Email         = u.Email ?? "",
                    CreatedAt     = u.CreatedAt,
                    Roles         = roles.ToList(),
                    SurveyCount   = u.Surveys.Count,
                    ResponseCount = u.Responses.Count
                });
            }

            return View(vmList);
        }

        // POST: Admin/ToggleRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (await _userManager.IsInRoleAsync(user, role))
                await _userManager.RemoveFromRoleAsync(user, role);
            else
                await _userManager.AddToRoleAsync(user, role);

            TempData["Success"] = $"Đã cập nhật quyền cho {user.Email}.";
            return RedirectToAction(nameof(Users));
        }

        // POST: Admin/DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (userId == currentUserId)
            {
                TempData["Warning"] = "Bạn không thể xóa chính mình!";
                return RedirectToAction(nameof(Users));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            TempData["Success"] = $"Đã xóa người dùng {user.Email}.";
            return RedirectToAction(nameof(Users));
        }

        // ─── EXPORT EXCEL ─────────────────────────────────────────────────────
        public async Task<IActionResult> ExportExcel(int surveyId)
        {
            var survey = await _context.Surveys
                .Include(s => s.Questions).ThenInclude(q => q.Options)
                .Include(s => s.Responses).ThenInclude(r => r.Answers).ThenInclude(a => a.SelectedOption)
                .Include(s => s.Responses).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(s => s.Id == surveyId);

            if (survey == null) return NotFound();

            using var wb = new XLWorkbook();

            // ── Sheet 1: Summary ──────────────────────────────────────────────
            var wsSummary = wb.Worksheets.Add("Tổng hợp");
            wsSummary.Cell(1, 1).Value = "Khảo sát:";
            wsSummary.Cell(1, 2).Value = survey.Title;
            wsSummary.Cell(2, 1).Value = "Mô tả:";
            wsSummary.Cell(2, 2).Value = survey.Description ?? "";
            wsSummary.Cell(3, 1).Value = "Tổng lượt trả lời:";
            wsSummary.Cell(3, 2).Value = survey.Responses.Count;
            wsSummary.Cell(4, 1).Value = "Số câu hỏi:";
            wsSummary.Cell(4, 2).Value = survey.Questions.Count;

            wsSummary.Column(1).Style.Font.Bold = true;
            wsSummary.Column(1).Width = 22;
            wsSummary.Column(2).Width = 40;

            // ── Sheet 2: Chi tiết câu hỏi trắc nghiệm ─────────────────────────
            var wsStats = wb.Worksheets.Add("Thống kê câu hỏi");
            int row = 1;
            int qNum = 1;
            foreach (var q in survey.Questions.Where(q => q.QuestionType == Models.QuestionType.MultipleChoice))
            {
                wsStats.Cell(row, 1).Value = $"Câu {qNum++}: {q.Text}";
                wsStats.Cell(row, 1).Style.Font.Bold = true;
                wsStats.Cell(row, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                wsStats.Range(row, 1, row, 3).Merge();
                row++;

                wsStats.Cell(row, 1).Value = "Lựa chọn";
                wsStats.Cell(row, 2).Value = "Số lượt chọn";
                wsStats.Cell(row, 3).Value = "Tỉ lệ (%)";
                wsStats.Row(row).Style.Font.Bold = true;
                wsStats.Row(row).Style.Fill.BackgroundColor = XLColor.LightYellow;
                row++;

                int totalAnswered = survey.Responses.SelectMany(r => r.Answers)
                    .Count(a => a.QuestionId == q.Id && a.SelectedOptionId.HasValue);

                foreach (var opt in q.Options)
                {
                    int cnt = survey.Responses.SelectMany(r => r.Answers)
                        .Count(a => a.SelectedOptionId == opt.Id);
                    double pct = totalAnswered > 0 ? Math.Round((double)cnt / totalAnswered * 100, 1) : 0;
                    wsStats.Cell(row, 1).Value = opt.Text;
                    wsStats.Cell(row, 2).Value = cnt;
                    wsStats.Cell(row, 3).Value = pct;
                    row++;
                }
                row++;
            }
            wsStats.Column(1).Width = 40;
            wsStats.Column(2).Width = 18;
            wsStats.Column(3).Width = 14;

            // ── Sheet 3: Dữ liệu thô ──────────────────────────────────────────
            var wsRaw = wb.Worksheets.Add("Dữ liệu thô");
            wsRaw.Cell(1, 1).Value = "STT";
            wsRaw.Cell(1, 2).Value = "Email người trả lời";
            wsRaw.Cell(1, 3).Value = "Thời gian nộp";
            int col = 4;
            var questionList = survey.Questions.ToList();
            foreach (var q in questionList)
            {
                wsRaw.Cell(1, col).Value = q.Text;
                col++;
            }
            wsRaw.Row(1).Style.Font.Bold = true;
            wsRaw.Row(1).Style.Fill.BackgroundColor = XLColor.LightBlue;

            int dataRow = 2;
            int stt = 1;
            foreach (var resp in survey.Responses.OrderBy(r => r.SubmittedAt))
            {
                wsRaw.Cell(dataRow, 1).Value = stt++;
                wsRaw.Cell(dataRow, 2).Value = resp.User?.Email ?? "";
                wsRaw.Cell(dataRow, 3).Value = resp.SubmittedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                int c = 4;
                foreach (var q in questionList)
                {
                    var ans = resp.Answers.FirstOrDefault(a => a.QuestionId == q.Id);
                    if (ans == null) { wsRaw.Cell(dataRow, c).Value = ""; }
                    else if (ans.SelectedOption != null) wsRaw.Cell(dataRow, c).Value = ans.SelectedOption.Text;
                    else wsRaw.Cell(dataRow, c).Value = ans.TextAnswer ?? "";
                    c++;
                }
                dataRow++;
            }
            wsRaw.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Seek(0, SeekOrigin.Begin);

            string fileName = $"KhaoSat_{survey.Id}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            return File(ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
