# Chương 3: Clean Architecture và REST API cơ bản (EduHub Skeleton)

* **Môn học:** Phát triển phần mềm hướng đối tượng (IN4211)
* **Giảng viên hướng dẫn:** ThS. Nguyễn Tuấn Kiệt
* **Sinh viên thực hiện:** Phan Trần Gia Hưng
* **Mã số sinh viên (MSSV):** `0024417689`
* **Lớp:** ĐHCNTT24B-IT

---

## 1. Giới thiệu dự án

Dự án **EduHub** trong Chương 3 được tổ chức lại theo mô hình **Clean Architecture (4 lớp)** kết hợp **REST API Controller-based** trên nền tảng **.NET 10** và **C# 14**:

1. **`EduHub.Domain`**: Định nghĩa Entity (`Club`) và Interface Repository (`IClubRepository`). Lớp này là trung tâm của hệ thống và **hoàn toàn độc lập**, không có bất kỳ dòng using nào phụ thuộc ra bên ngoài.
2. **`EduHub.Application`**: Chứa các Use Case theo mô hình CQRS tối giản (Command/Query + Handler + DTO) để xử lý logic nghiệp vụ độc lập:
   - `GetClubsQueryHandler`: Lấy danh sách câu lạc bộ.
   - `GetClubByIdQueryHandler`: Lấy thông tin câu lạc bộ theo Id.
   - `CreateClubCommandHandler`: Kiểm tra tính hợp lệ của tên CLB, khởi tạo và lưu trữ.
   - `DeleteClubCommandHandler`: Xoá câu lạc bộ khỏi hệ thống.
3. **`EduHub.Infrastructure`**: Cài đặt lớp lưu trữ dữ liệu cụ thể (`InMemoryClubRepository`) kế thừa `IClubRepository` (dùng cho Ch03 khi chưa tích hợp CSDL PostgreSQL). Sử dụng cơ chế lock an toàn để xử lý đồng thời.
4. **`EduHub.API`**: Cung cấp các RESTful API endpoints thông qua `ClubsController`, tích hợp tài liệu tương tác Swagger UI và đăng ký Dependency Injection (DI) trong `Program.cs`.
5. **`EduHub.Tests`**: Kiểm thử tự động (Unit Test) lớp Application bằng `xUnit` và thư viện giả lập `Moq` để cô lập repository.

---

## 2. Kết quả hoàn thành các TODO

- [x] **TODO 1 (1a - 1e):** Cài đặt đầy đủ 5 phương thức trong `InMemoryClubRepository.cs` (`GetAllAsync`, `GetByIdAsync`, `AddAsync`, `UpdateAsync`, `DeleteAsync`).
- [x] **TODO 2:** Viết logic lấy danh sách CLB trong `GetClubsQueryHandler.cs`.
- [x] **TODO 3:** Viết logic tìm CLB theo Id trong `GetClubByIdQueryHandler.cs` (trả về null nếu không tồn tại).
- [x] **TODO 4 (4a - 4d):** Kiểm tra tên rỗng (`ArgumentException`), khởi tạo Club và gọi `AddAsync` trong `CreateClubCommandHandler.cs`.
- [x] **TODO 5:** Gọi `DeleteAsync` trong `DeleteClubCommandHandler.cs`.
- [x] **TODO 6 (6a - 6c):** Đăng ký DI cho `IClubRepository` và 4 Handler trong `Program.cs`.
- [x] **TODO 7:** Cấu hình SwaggerGen và SwaggerUI trong `Program.cs`.
- [x] **TODO 8 (8a - 8i):** Hoàn thiện 4 Action method trong `ClubsController.cs` với HTTP status code chuẩn:
  - `GET /api/clubs` -> 200 OK
  - `GET /api/clubs/{id}` -> 200 OK hoặc 404 Not Found
  - `POST /api/clubs` -> 201 Created (kèm header Location trỏ đến endpoint GetById) hoặc 400 Bad Request
  - `DELETE /api/clubs/{id}` -> 204 NoContent hoặc 404 Not Found
- [x] **Unit Test bổ sung:** Viết thêm test case `GetClubById_WhenIdDoesNotExist_ReturnsNull` sử dụng Moq (đạt 6/6 test cases passed).

---

## 3. Hướng dẫn chạy và kiểm thử

### Kiểm tra build toàn bộ Solution
```bash
dotnet build EduHub.sln
```

### Chạy kiểm thử tự động (Unit Test)
```bash
dotnet test
```

### Khởi chạy REST API và mở Swagger UI
```bash
dotnet run --project EduHub.API
```
Truy cập Swagger UI tại trình duyệt: `http://localhost:5000/swagger` (hoặc cổng cấu hình của IDE).
