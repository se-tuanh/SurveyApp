# Hệ thống quản lý Khảo sát trực tuyến (Survey Management App)

# 🚀 Công nghệ sử dụng
- **Ngôn ngữ:** C# 12
- **Framework:** ASP.NET Core 8 MVC
- **Database:** SQL Server (sử dụng LocalDB cho môi trường phát triển)
- **ORM:** Entity Framework Core 8
- **Authentication:** ASP.NET Core Identity (phân quyền Role-based Admin/User)
- **Giao diện:** HTML5, CSS3, Bootstrap 5, Bootstrap Icons
- **Thư viện bên thứ 3:** `ClosedXML` (để xuất dữ liệu thống kê ra file Excel)

Các tính năng chính
# Dành cho người dùng thường (User)
- Đăng ký tài khoản, đăng nhập, đăng xuất hệ thống.
- Xem danh sách các khảo sát đang được mở trên trang chủ.
- **Tham gia khảo sát:** Trả lời các câu hỏi dạng Trắc nghiệm (chỉ chọn 1 đáp án môt) hoặc Tự luận (nhập văn bản).
- **Quản lý khảo sát cá nhân:** Tự do tạo, sửa, xóa, và đóng/mở khảo sát của do chính mình tạo ra.
- **Thiết kế câu hỏi:** Thêm, sửa, xóa các câu hỏi (tự luận/trắc nghiệm) bên trong khảo sát của mình. Thêm các lựa chọn (options) cho loại câu trắc nghiệm.
- Xem biểu đồ thống kê phần trăm câu trả lời, lượt tham gia khảo sát.

# Dành cho Quản trị viên (Admin)
- Admin có mọi quyền hạn của người dùng bình thường và có thể tham gia/thiết kế khảo sát.
- **Admin Dashboard:** Bảng điều khiển tổng quan thống kê số lượng người dùng, tổng số khảo sát đang hoạt động, hệ thống hoạt động gần đây.
- **Quản lý người dùng:** Liệt kê các thành viên trong hệ thống. Admin có thể Cấp/Hủy quyền quản trị viên cho bất kỳ ai, và xóa tài khoản người dùng khỏi hệ thống.
- **Xuất Excel:** Toàn bộ kết quả khảo sát (dữ liệu thô và thống kê) có thể xuất khẩu ra file Excel (.xlsx).


