using Microsoft.AspNetCore.Identity;
using SurveyApp.Models;

namespace SurveyApp.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@deptrai.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Administrator",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            if (!context.Surveys.Any(s => s.Title.Contains("Căn tin")))
            {
                var survey1 = new Survey
                {
                    Title = "Khảo sát chất lượng dịch vụ Căn tin sinh viên",
                    Description = "Nhằm cải thiện chất lượng phục vụ và vệ sinh an toàn thực phẩm, phòng Công tác Sinh viên tổ chức khảo sát ý kiến toàn thể sinh viên về căn tin trường.",
                    CreatedById = adminUser.Id,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    IsActive = true,
                    ClosedAt = DateTime.Now.AddDays(10)
                };

                var survey2 = new Survey
                {
                    Title = "Đánh giá môn học Công nghệ phần mềm kỳ I",
                    Description = "Khảo sát nhanh để thu thập ý kiến sinh viên về phương pháp giảng dạy và bài tập lớn môn CNPM.",
                    CreatedById = adminUser.Id,
                    CreatedAt = DateTime.Now.AddDays(-10),
                    IsActive = true,
                    ClosedAt = DateTime.Now.AddDays(-1)
                };

                context.Surveys.AddRange(survey1, survey2);
                await context.SaveChangesAsync();

                var q1_1 = new Question { SurveyId = survey1.Id, Text = "Tần suất bạn ăn tại căn tin?", QuestionType = QuestionType.MultipleChoice };
                var q1_2 = new Question { SurveyId = survey1.Id, Text = "Giá cả các món ăn tại căn tin theo bạn là như thế nào?", QuestionType = QuestionType.MultipleChoice };
                var q1_3 = new Question { SurveyId = survey1.Id, Text = "Bạn có góp ý gì để căn tin phục vụ tốt hơn không?", QuestionType = QuestionType.Text };

                var q2_1 = new Question { SurveyId = survey2.Id, Text = "Bạn thấy khối lượng kiến thức của môn học thế nào?", QuestionType = QuestionType.MultipleChoice };
                var q2_2 = new Question { SurveyId = survey2.Id, Text = "Mức độ hài lòng của bạn về bài tập lớn (Project)?", QuestionType = QuestionType.MultipleChoice };

                context.Questions.AddRange(q1_1, q1_2, q1_3, q2_1, q2_2);
                await context.SaveChangesAsync();

                context.Options.AddRange(
                    new Option { QuestionId = q1_1.Id, Text = "Hàng ngày" },
                    new Option { QuestionId = q1_1.Id, Text = "Vài lần một tuần" },
                    new Option { QuestionId = q1_1.Id, Text = "Hiếm khi" },
                    new Option { QuestionId = q1_1.Id, Text = "Chưa bao giờ" },
                    
                    new Option { QuestionId = q1_2.Id, Text = "Rẻ, hợp lý" },
                    new Option { QuestionId = q1_2.Id, Text = "Bình thường" },
                    new Option { QuestionId = q1_2.Id, Text = "Hơi đắt" },
                    new Option { QuestionId = q1_2.Id, Text = "Quá đắt so với sinh viên" },

                    new Option { QuestionId = q2_1.Id, Text = "Vừa sức, dễ tiếp thu" },
                    new Option { QuestionId = q2_1.Id, Text = "Khá nhiều nhưng vẫn ổn" },
                    new Option { QuestionId = q2_1.Id, Text = "Bị quá tải, lý thuyết nhiều" },

                    new Option { QuestionId = q2_2.Id, Text = "Rất hài lòng, thực tế" },
                    new Option { QuestionId = q2_2.Id, Text = "Bình thường" },
                    new Option { QuestionId = q2_2.Id, Text = "Chưa rõ ràng yêu cầu" }
                );
                await context.SaveChangesAsync();

                var fakeUser = new ApplicationUser { UserName = "sv01@student.edu.vn", Email = "sv01@student.edu.vn", FullName = "Nguyễn Văn A", EmailConfirmed = true };
                var fakeUser2 = new ApplicationUser { UserName = "sv02@student.edu.vn", Email = "sv02@student.edu.vn", FullName = "Trần Thị B", EmailConfirmed = true };
                
                if (await userManager.FindByEmailAsync(fakeUser.Email) == null)
                    await userManager.CreateAsync(fakeUser, "Sinhvien@123");
                else
                    fakeUser = await userManager.FindByEmailAsync(fakeUser.Email);

                if (await userManager.FindByEmailAsync(fakeUser2.Email) == null)
                    await userManager.CreateAsync(fakeUser2, "Sinhvien@123");
                else
                    fakeUser2 = await userManager.FindByEmailAsync(fakeUser2.Email);

                var r1 = new Response { SurveyId = survey1.Id, UserId = fakeUser.Id, SubmittedAt = DateTime.Now.AddHours(-5) };
                var r2 = new Response { SurveyId = survey2.Id, UserId = fakeUser2.Id, SubmittedAt = DateTime.Now.AddDays(-3) };
                context.Responses.AddRange(r1, r2);
                await context.SaveChangesAsync();

                var opts_q1_1 = context.Options.Where(o => o.QuestionId == q1_1.Id).ToList();
                var opts_q1_2 = context.Options.Where(o => o.QuestionId == q1_2.Id).ToList();
                var opts_q2_1 = context.Options.Where(o => o.QuestionId == q2_1.Id).ToList();
                var opts_q2_2 = context.Options.Where(o => o.QuestionId == q2_2.Id).ToList();

                context.Answers.AddRange(
                    new Answer { ResponseId = r1.Id, QuestionId = q1_1.Id, SelectedOptionId = opts_q1_1[1].Id },
                    new Answer { ResponseId = r1.Id, QuestionId = q1_2.Id, SelectedOptionId = opts_q1_2[2].Id },
                    new Answer { ResponseId = r1.Id, QuestionId = q1_3.Id, TextAnswer = "Nên thêm các món nước và đồ ăn vặt đa dạng hơn ạ." },

                    new Answer { ResponseId = r2.Id, QuestionId = q2_1.Id, SelectedOptionId = opts_q2_1[1].Id },
                    new Answer { ResponseId = r2.Id, QuestionId = q2_2.Id, SelectedOptionId = opts_q2_2[0].Id }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
