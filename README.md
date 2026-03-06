# Hệ thống quản lý Khảo sát trực tuyến (Survey Management App)

Đây là một dự án ứng dụng web dùng để quản lý khảo sát trực tuyến, được phát triển bằng **C#** và **ASP.NET Core MVC**. Dự án này là bài tập môn học (PRN222).

## 🚀 Công nghệ sử dụng
- **Ngôn ngữ:** C# 12
- **Framework:** ASP.NET Core 8 MVC
- **Database:** SQL Server (sử dụng LocalDB cho môi trường phát triển)
- **ORM:** Entity Framework Core 8
- **Authentication:** ASP.NET Core Identity (phân quyền Role-based Admin/User)
- **Giao diện:** HTML5, CSS3, Bootstrap 5, Bootstrap Icons
- **Thư viện bên thứ 3:** `ClosedXML` (để xuất dữ liệu thống kê ra file Excel)

## ✨ Các tính năng chính

### Dành cho người dùng thường (User)
- Đăng ký tài khoản, đăng nhập, đăng xuất hệ thống.
- Xem danh sách các khảo sát đang được mở trên trang chủ.
- **Tham gia khảo sát:** Trả lời các câu hỏi dạng Trắc nghiệm (chỉ chọn 1 đáp án môt) hoặc Tự luận (nhập văn bản).
- **Quản lý khảo sát cá nhân:** Tự do tạo, sửa, xóa, và đóng/mở khảo sát của do chính mình tạo ra.
- **Thiết kế câu hỏi:** Thêm, sửa, xóa các câu hỏi (tự luận/trắc nghiệm) bên trong khảo sát của mình. Thêm các lựa chọn (options) cho loại câu trắc nghiệm.
- Xem biểu đồ thống kê phần trăm câu trả lời, lượt tham gia khảo sát.

### Dành cho Quản trị viên (Admin)
- Admin có mọi quyền hạn của người dùng bình thường và có thể tham gia/thiết kế khảo sát.
- **Admin Dashboard:** Bảng điều khiển tổng quan thống kê số lượng người dùng, tổng số khảo sát đang hoạt động, hệ thống hoạt động gần đây.
- **Quản lý người dùng:** Liệt kê các thành viên trong hệ thống. Admin có thể Cấp/Hủy quyền quản trị viên cho bất kỳ ai, và xóa tài khoản người dùng khỏi hệ thống.
- **Xuất Excel:** Toàn bộ kết quả khảo sát (dữ liệu thô và thống kê) có thể xuất khẩu ra file Excel (.xlsx).

## 💻 Hướng dẫn chạy dự án (Getting Started)

### Yêu cầu hệ thống
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server LocalDB (thường được cài sẵn khi cài Visual Studio có workload ASP.NET)
- Visual Studio 2022 hoặc Visual Studio Code.

### Cài đặt và khởi chạy (Command Line)

1. Mở Terminal / PowerShell ở thư mục gốc của dự án (nơi chứa file `.sln` hoặc `.csproj`).
2. Restore các packages cần thiết:
   ```bash
   dotnet restore
   ```
3. Cập nhật Database (tạo CSDL và các bảng ban đầu):
   ```bash
   dotnet ef database update
   ```
4. Khởi chạy ứng dụng web:
   ```bash
   dotnet run
   ```
5. Ứng dụng sẽ chạy tại địa chỉ: `https://localhost:5001` và/hoặc `http://localhost:5000`. Hãy mở trình duyệt truy cập vào một trong hai đường link trên.

### 🔑 Tài khoản khởi tạo mặc định (Seeding Data)
Khi ứng dụng khởi chạy lần đầu tiên và được tạo Database, hệ thống sẽ tự động tạo Roles `Admin`, `User` cùng 1 tài khoản Admin mặc định để bạn đăng nhập:

- **Email:** `admin@survey.com`
- **Password:** `Admin@123`

*(Bạn nên thay đổi mật khẩu sau khi đăng nhập thành công hoặc tạo tài khoản mới rồi cấp quyền Admin ở phần Quản lý người dùng).*

## 📁 Cấu trúc thư mục 
- `Controllers/`: Mọi Controller xử lý logic điều hướng (Home, Account, Survey, Admin, Response, Statistics...).
- `Models/`: Các file Entity mapping với Database.
- `ViewModels/`: Các class DTO truyền tải dữ liệu giữa Views và Controllers.
- `Views/`: Mã nguồn giao diện (.cshtml).
- `Data/`: Chứa file cấu hình EntityFramework (`ApplicationDbContext`) và logic Seed dữ liệu mặc định (`SeedData.cs`).

---
*Developed as a class project for PRN222.*
