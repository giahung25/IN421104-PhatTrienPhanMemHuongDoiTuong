# Refactor EduHub với 5 nguyên lý SOLID và 4 Design Patterns

## 1. What (Các thay đổi đã thực hiện)
- **Nhiệm vụ 1 (Phân tích SOLID):** Lập tài liệu `docs/solid-analysis.md` chỉ rõ các vi phạm SRP, OCP, DIP, ISP trong lớp `ClubManager.cs`.
- **Nhiệm vụ 2 (Tách lớp & Interface):**
  - Trừu tượng hóa ghi log qua `IAppLogger` (`ConsoleLogger`, `FileLogger`).
  - Tách kho dữ liệu qua `IClubRepository`, `IMemberRepository`, `IEventRepository` và các lớp InMemory tương ứng.
  - Phân rã nghiệp vụ thành `ClubService`, `EventService`, `ReportService`.
- **Nhiệm vụ 3 (4 Design Patterns):**
  - **Factory Method:** `INotificationServiceFactory` khởi tạo dịch vụ thông báo (`Email`, `Sms`, `Slack`) theo `NotificationChannel`.
  - **Decorator:** `LoggingClubRepository` bọc ngoài `IClubRepository` để ghi log trong suốt.
  - **Strategy:** `IReportStrategy` hỗ trợ xuất báo cáo định dạng `ConsoleReportStrategy` và `CsvReportStrategy`.
  - **Observer:** `IEventObserver` với `EmailNotifier` và `LogNotifier` tự động nhận tin khi có sự kiện mới.
- **Nhiệm vụ 4 (Tầng Domain & Unit Test xUnit):**
  - Đóng gói quy tắc nghiệp vụ trong `Club.cs` (`Create`, `HasRoom`) và `Event.cs` (`Register`, `CheckIn`, `AttendanceRate`).
  - Viết 7 unit test xUnit chuẩn AAA trong `DomainTests.cs` (kiểm tra ngoại lệ và giá trị biên, tỷ lệ đạt 7/7 passed 100%).
- **Nhiệm vụ 5 (Composition Root & Quy trình Git):**
  - Cập nhật `Program.cs` cấu hình Pure Dependency Injection hoàn chỉnh.
  - Tuân thủ quy trình Git branching `feature/solid-refactor` và Conventional Commits.

## 2. Why (Lý do thực hiện)
- Mã nguồn ban đầu gom toàn bộ nghiệp vụ vào God Class `ClubManager`, vi phạm nghiêm trọng các nguyên lý SOLID:
  - Khó bảo trì, sửa đổi một tính năng dễ làm lỗi các tính năng khác.
  - Phụ thuộc trực tiếp vào I/O phần cứng (`File.AppendAllText`, `Console.WriteLine`), không thể kiểm thử cô lập.
  - Khó mở rộng thêm kênh thông báo hay định dạng báo cáo mới.
- Việc tái cấu trúc giúp mã nguồn đạt tính kết dính cao, khớp nối lỏng (Loose Coupling), dễ bảo trì và dễ mở rộng.

## 3. How to test (Cách kiểm thử và xác minh)
```bash
# 1. Biên dịch dự án
dotnet build ch02/Ch02.Refactor.sln

# 2. Chạy toàn bộ kiểm thử tự động
dotnet test ch02/Ch02.Refactor.sln

# 3. Chạy demo kịch bản nghiệp vụ qua console
dotnet run --project ch02/Ch02.Refactor
```
Kết quả mong đợi:
- Tất cả các bài kiểm thử đều vượt qua (7/7 Passed).
- Kịch bản demo trên Console hiển thị đầy đủ các luồng tạo CLB, gửi thông báo qua Observer, xuất báo cáo qua Strategy và Factory thông báo.
