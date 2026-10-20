# Báo cáo phân tích vi phạm SOLID trong lớp ClubManager

**Học phần:** Phát triển phần mềm hướng đối tượng (IN4211)  
**Sinh viên thực hiện:** Phan Trần Gia Hưng  
**Mã số sinh viên:** 0024417689  
**Lớp:** DHCNTT24B  
**Nội dung:** Nhiệm vụ 1 (TODO 1) — Phân tích các vi phạm nguyên lý SOLID trong lớp `ClubManager.cs`

---

## 1. Tổng quan về mã nguồn ban đầu

Trong mã nguồn khởi điểm của dự án EduHub (chương 02), lớp `ClubManager` được thiết kế theo dạng "God class". Lớp này tập trung hầu hết mọi thao tác của hệ thống câu lạc bộ vào một nơi duy nhất:
- Kiểm tra tính hợp lệ của dữ liệu đầu vào (tên CLB, độ dài, kiểm tra sĩ số).
- Quản lý danh sách câu lạc bộ và thành viên lưu trữ tạm thời trong bộ nhớ.
- Trực tiếp mở file và ghi log ra ổ đĩa (`club-log.txt`).
- Gửi thông báo đến người dùng bằng cách in trực tiếp ra màn hình Console.
- Tính toán số liệu thống kê và in báo cáo.

Việc dồn quá nhiều trách nhiệm vào một lớp duy nhất khiến mã nguồn trở nên cồng kềnh, khó bảo trì, khó mở rộng và gần như không thể viết unit test một cách độc lập. Dưới đây là phân tích chi tiết các vi phạm nguyên lý SOLID được tìm thấy trong mã nguồn.

---

## 2. Chi tiết các vi phạm nguyên lý SOLID

### 2.1. Vi phạm nguyên lý đơn trách nhiệm (Single Responsibility Principle - SRP)

Nguyên lý SRP phát biểu rằng mỗi một lớp hoặc phương thức chỉ nên đảm nhận một trách nhiệm duy nhất và chỉ có một lý do duy nhất để thay đổi.

**Vị trí vi phạm trong mã nguồn:**
- Phương thức `CreateClub` (dòng 35 - 64)
- Phương thức `AddMemberToClub` (dòng 67 - 84)
- Phương thức `CreateEvent` (dòng 88 - 99)
- Phương thức `PrintClubReport` (dòng 103 - 113)

**Phân tích chi tiết:**
Trong lớp `ClubManager`, hầu hết các phương thức đều đang ôm đồm nhiều công việc khác nhau:
- Phương thức `CreateClub`: vừa kiểm tra tính hợp lệ của tên câu lạc bộ (rỗng hay quá 100 ký tự), vừa khởi tạo đối tượng `Club` và thêm vào danh sách nội bộ, vừa ghi log ra file `club-log.txt`, lại vừa chứa logic phân loại email để gửi thông báo cho chủ nhiệm.
- Phương thức `AddMemberToClub`: vừa tìm kiếm câu lạc bộ, vừa kiểm tra giới hạn sĩ số (tối đa 50 người), vừa lưu thành viên, vừa ghi file log và in thông báo chào mừng cùng thông báo Slack ra màn hình.
- Phương thức `CreateEvent`: vừa tạo sự kiện, vừa ghi log và tự động duyệt vòng lặp gửi email đến từng thành viên trong câu lạc bộ.
- Phương thức `PrintClubReport`: kết hợp cả việc tổng hợp số lượng thành viên, in định dạng ra màn hình và ghi log hoạt động xuất báo cáo.

**Hậu quả:**
Lớp `ClubManager` có quá nhiều lý do để phải thay đổi. Khi có bất kỳ yêu cầu nào phát sinh liên quan đến: thay đổi định dạng log, thay đổi quy tắc kiểm tra dữ liệu, đổi cách hiển thị báo cáo hay chuyển kênh gửi thông báo... lập trình viên đều phải can thiệp trực tiếp vào file này. Điều này làm tăng độ phức tạp khi đọc hiểu mã nguồn và dễ gây ra lỗi ngoài ý muốn cho các chức năng khác khi sửa đổi.

**Hướng xử lý khi refactor:**
- Chuyển các quy tắc nghiệp vụ và kiểm tra tính toàn vẹn dữ liệu về đúng các thực thể trong tầng Domain (`Club.Create`, `Club.HasRoom`, `Event.Register`).
- Tách việc điều phối nghiệp vụ thành các dịch vụ riêng biệt: `ClubService`, `EventService`, `ReportService`.
- Tách tác vụ ghi log và gửi thông báo ra các thành phần chuyên trách (`IAppLogger`, `INotificationService`).

---

### 2.2. Vi phạm nguyên lý Đóng/Mở (Open/Closed Principle - OCP)

Nguyên lý OCP yêu cầu các thực thể phần mềm (lớp, module, hàm) nên mở cho việc mở rộng (thêm tính năng mới), nhưng đóng đối với việc chỉnh sửa mã nguồn hiện có.

**Vị trí vi phạm trong mã nguồn:**
- Đoạn phân nhánh gửi thông báo trong `CreateClub` (dòng 52 - 63):
  ```csharp
  if (presidentEmail.Contains("@gmail.com"))
  {
      Console.WriteLine($"Sending Gmail notification to {presidentEmail}");
  }
  else if (presidentEmail.Contains("@yahoo.com"))
  {
      Console.WriteLine($"Sending Yahoo notification to {presidentEmail}");
  }
  else
  {
      Console.WriteLine($"Sending generic email to {presidentEmail}");
  }
  ```
- Logic thông báo cố định trong `AddMemberToClub` (dòng 82 - 83) và `CreateEvent` (dòng 96 - 97).
- In báo cáo cố định trong `PrintClubReport` (dòng 103 - 113).

**Phân tích chi tiết:**
- Đoạn code gửi thông báo cho chủ nhiệm CLB đang dùng cấu trúc `if-else` so sánh chuỗi email. Nếu sau này hệ thống cần gửi thông báo qua SMS, thông báo ứng dụng, hoặc hỗ trợ thêm các dịch vụ email khác như Outlook, ta bắt buộc phải mở phương thức `CreateClub` ra để sửa hoặc thêm nhánh `else if`.
- Tương tự, phương thức `PrintClubReport` đang gắn cứng định dạng in ra màn hình Console. Nếu khách hàng muốn xuất báo cáo dưới dạng tệp CSV, file PDF hay dữ liệu JSON, ta lại phải chỉnh sửa lại mã nguồn của phương thức này hoặc viết thêm các phương thức in khác vào trong lớp.

**Hậu quả:**
Mỗi lần mở rộng tính năng hay thay đổi cấu hình hệ thống đều buộc phải can thiệp vào các đoạn mã cũ đang hoạt động ổn định. Điều này làm tăng nguy cơ phát sinh lỗi ngoài ý muốn (lỗi hồi quy) trên các tính năng hiện hành.

**Hướng xử lý khi refactor:**
- Áp dụng mẫu thiết kế Factory (`INotificationServiceFactory`): tập trung việc khởi tạo dịch vụ thông báo dựa trên kênh (`NotificationChannel`), nghiệp vụ tạo câu lạc bộ chỉ cần gọi gửi thông báo mà không cần quan tâm đến chi tiết triển khai bên dưới.
- Áp dụng mẫu thiết kế Strategy (`IReportStrategy`): định nghĩa giao diện chung cho việc xuất báo cáo với các chiến lược cụ thể như `ConsoleReportStrategy`, `CsvReportStrategy`. Khi cần thêm định dạng mới, ta chỉ cần tạo một lớp mới cài đặt `IReportStrategy` mà không cần sửa code cũ.
- Áp dụng mẫu thiết kế Observer (`IEventObserver`): khi một sự kiện mới được tạo ra, `EventService` sẽ thông báo cho danh sách các observer đã đăng ký mà không cần hardcode vòng lặp gửi email trong hàm tạo sự kiện.

---

### 2.3. Vi phạm nguyên lý đảo ngược phụ thuộc (Dependency Inversion Principle - DIP)

Nguyên lý DIP gồm hai ý chính:
1. Các module cấp cao không nên phụ thuộc vào các module cấp thấp; cả hai nên phụ thuộc vào sự trừu tượng (abstraction / interface).
2. Sự trừu tượng không nên phụ thuộc vào chi tiết, mà chi tiết phải phụ thuộc vào sự trừu tượng.

**Vị trí vi phạm trong mã nguồn:**
- Thao tác ghi file trực tiếp thông qua API tĩnh của .NET: `File.AppendAllText(_logFilePath, ...)` tại các dòng 48, 79, 94, 111.
- In trực tiếp ra console: `Console.WriteLine(...)` tại các dòng 54, 58, 62, 82, 83, 97, 108.
- Tự khởi tạo và quản lý trực tiếp danh sách lưu trữ bằng biến nội bộ `new List<Club>()` và `new List<Member>()`.

**Phân tích chi tiết:**
Lớp `ClubManager` là nơi nắm giữ các quy tắc điều phối nghiệp vụ (module cấp cao), nhưng lại trực tiếp gọi các API cấp thấp của hệ điều hành như hệ thống tệp tin (`System.IO.File`) và luồng xuất chuẩn (`System.Console`). Thay vì làm việc thông qua một giao diện trừu tượng mô tả hành vi ghi log hay lưu trữ dữ liệu, lớp này gắn chặt việc thực thi của mình vào việc ghi đĩa và in console.

**Hậu quả:**
- **Rất khó viết Unit Test:** Khi kiểm thử các chức năng nghiệp vụ của `ClubManager` (ví dụ kiểm tra việc thêm câu lạc bộ), đoạn code sẽ tự động tạo và ghi nội dung vào file `club-log.txt` trên ổ cứng thật cũng như in văn bản ra console. Điều này làm các bài kiểm thử bị chậm, phụ thuộc vào quyền ghi tập tin của hệ thống, để lại dữ liệu rác sau khi test và không thể giả lập (mock) các tình huống ngoại lệ như lỗi đĩa đầy hay lỗi không có quyền truy cập.
- **Tính linh hoạt kém:** Khi muốn đổi từ việc ghi log ra file sang ghi log vào cơ sở dữ liệu hoặc dịch vụ đám mây, ta buộc phải viết lại mã nguồn của `ClubManager`.

**Hướng xử lý khi refactor:**
- Trừu tượng hóa thao tác ghi log bằng interface `IAppLogger`, cho phép triển khai linh hoạt (`ConsoleLogger`, `FileLogger`, hoặc test mock).
- Trừu tượng hóa việc lưu trữ dữ liệu bằng các interface repository (`IClubRepository`, `IMemberRepository`, `IEventRepository`).
- Sử dụng cơ chế tiêm phụ thuộc qua hàm khởi tạo (Constructor Injection): các service chỉ nhận abstraction từ bên ngoài, toàn bộ việc lắp ghép các phụ thuộc cụ thể được chuyển về hàm `Main` trong `Program.cs`.

---

### 2.4. Vi phạm nguyên lý phân tách giao diện (Interface Segregation Principle - ISP)

Nguyên lý ISP phát biểu rằng các client không nên bị ép buộc phải phụ thuộc vào những giao diện mà chúng không thực sự sử dụng. Thay vì tạo ra một giao diện chung quá lớn, ta nên chia nhỏ thành nhiều giao diện chuyên biệt.

**Vị trí vi phạm trong mã nguồn:**
- Toàn bộ lớp `ClubManager` không kế thừa bất kỳ interface nào và gom chung mọi nghiệp vụ của CLB, thành viên, sự kiện và báo cáo vào một class duy nhất (dòng 28 - 117).

**Phân tích chi tiết:**
Hiện tại, `ClubManager` cung cấp một tập hợp phương thức đồ sộ bao gồm `CreateClub`, `GetAllClubs`, `AddMemberToClub`, `CreateEvent`, `PrintClubReport`. Giả sử hệ thống có một thành phần khác chỉ quan tâm đến việc tra cứu thông tin câu lạc bộ (ví dụ: màn hình tìm kiếm cho sinh viên), thành phần đó vẫn buộc phải phụ thuộc vào toàn bộ `ClubManager` với đầy đủ các phương thức ghi log, gửi mail, in báo cáo mà nó không hề cần đến.

**Hậu quả:**
- Độ kết dính (coupling) giữa các module trong hệ thống bị đẩy lên quá cao.
- Khó tái sử dụng mã nguồn cho các mục đích khác nhau hoặc trên các nền tảng khác nhau (giao diện web, ứng dụng di động, giao diện console hay tác vụ chạy nền).
- Khi có bất kỳ thay đổi nào trong phương thức không liên quan, các client phụ thuộc vào lớp cũng có nguy cơ phải biên dịch lại hoặc bị ảnh hưởng.

**Hướng xử lý khi refactor:**
- Chia nhỏ hệ thống thành các interface hẹp, tập trung đúng vào từng nhóm nghiệp vụ cụ thể:
  - Nhóm kho dữ liệu: `IClubRepository`, `IMemberRepository`, `IEventRepository`.
  - Nhóm dịch vụ điều phối: `IClubService`, `IEventService`, `IReportService`.
- Mỗi client hoặc mỗi thành phần chỉ cần phụ thuộc vào đúng interface cung cấp tính năng mà mình cần sử dụng.

---

## 3. Kế hoạch tái cấu trúc theo yêu cầu bài tập

Dựa trên các vi phạm đã chỉ ra, kế hoạch refactor cho bài tập chương 02 sẽ được triển khai theo các bước cụ thể:

1. **Chuẩn hóa tầng Domain (`Domain/Club.cs`, `Domain/Event.cs`):**
   Đưa các quy tắc kiểm tra tính hợp lệ dữ liệu và trạng thái vào chính thực thể để đảm bảo tính đóng gói (`Club.Create`, `Club.HasRoom`, `Event.Register`, `Event.CheckIn`).

2. **Phân rã lớp `ClubManager` thành các Service và Repository riêng biệt:**
   Tạo các interface và class tương ứng (`ClubService`, `EventService`, `ReportService`, `IClubRepository`, `IAppLogger`, `INotificationService`), áp dụng Dependency Injection qua constructor.

3. **Hiện thực hóa 4 mẫu thiết kế (Design Patterns):**
   - **Factory Method:** Xây dựng `NotificationServiceFactory` để khởi tạo dịch vụ thông báo theo kênh, thay thế chuỗi `if-else`.
   - **Decorator:** Xây dựng `LoggingClubRepository` bọc bên ngoài `IClubRepository` để bổ sung tính năng ghi log mà không làm thay đổi logic lưu trữ ban đầu.
   - **Strategy:** Xây dựng `IReportStrategy` cùng hai chiến lược cụ thể là `ConsoleReportStrategy` và `CsvReportStrategy`.
   - **Observer:** Xây dựng `IEventObserver` với các observer như `EmailNotifier`, `LogNotifier` để tự động xử lý khi có sự kiện mới được tạo ra.

4. **Kiểm thử tự động bằng xUnit:**
   Hoàn thiện các ca kiểm thử trong `DomainTests.cs` theo mẫu Arrange - Act - Assert (AAA), kiểm tra đầy đủ các trường hợp ngoại lệ và giá trị biên để đảm bảo mã nguồn sau khi tái cấu trúc hoạt động chính xác.
