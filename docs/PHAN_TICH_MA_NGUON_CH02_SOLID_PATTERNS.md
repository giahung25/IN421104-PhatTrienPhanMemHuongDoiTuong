# 📘 BÁO CÁO PHÂN TÍCH TOÀN DIỆN MÃ NGUỒN CHƯƠNG 02
## HỌC PHẦN: PHÁT TRIỂN PHẦN MỀM HƯỚNG ĐỐI TƯỢNG (IN4211 / IN4221)
### Chủ đề: Tái Cấu Trúc EduHub với SOLID, Design Patterns & xUnit Testing
* **Giảng viên hướng dẫn:** ThS. Nguyễn Tuấn Kiệt — Khoa Công nghệ và Kỹ thuật, Trường Đại học Đồng Tháp
* **Sinh viên thực hiện:** Phan Trần Gia Hưng
* **Mã số sinh viên (MSSV):** `0024417689`
* **Lớp sinh hoạt:** DHCNTT24B (Mã lớp HP: IN421104)
* **Vị trí tài liệu gốc:** `G:\My Drive\Study\HK1_Nam_3\IN4211_Phat_Trien_Phan_Mem_Huong_Doi_Tuong\05_Tai_Lieu`

---

## 📌 MỤC LỤC
1. [Tổng Quan Gói Tài Liệu `05_Tai_Lieu`](#1-tổng-quan-gói-tài-liệu-05_tai_lieu)
2. [Phần I: Phân Tích Mã Nguồn Khởi Điểm (`starter/` — `Ch02.Refactor.sln`)](#phần-i-phân-tích-mã-nguồn-khởi-điểm-starter--ch02refactorsln)
   * [1. Hiện trạng God Class `ClubManager.cs` và các vi phạm SOLID](#1-hiện-trạng-god-class-clubmanagercs-và-các-vi-phạm-solid)
   * [2. Hiện trạng Tầng Domain (`Club.cs`, `Event.cs`)](#2-hiện-trạng-tầng-domain-clubcs-eventcs)
   * [3. Hiện trạng Tầng Kiểm Thử (`DomainTests.cs`)](#3-hiện-trạng-tầng-kiểm-thử-domaintestscs)
3. [Phần II: Phân Tích Mã Nguồn Đáp Án Chuẩn (`solution/` — `EduHub.Ch02.sln`)](#phần-ii-phân-tích-mã-nguồn-đáp-án-chuẩn-solution--eduhubch02sln)
   * [1. Cấu trúc tổng thể sau tái cấu trúc (Layered Clean Design)](#1-cấu-trúc-tổng-thể-sau-tái-cấu-trúc-layered-clean-design)
   * [2. Hiện thực hóa 4 Design Patterns kinh điển](#2-hiện-thực-hóa-4-design-patterns-kinh-điển)
     * [Factory Method Pattern (Hệ thống thông báo)](#-pattern-1-factory-method-notifications)
     * [Decorator Pattern (Logging Repository trong suốt)](#-pattern-2-decorator-loggingclubrepository)
     * [Strategy Pattern (Đa dạng hóa định dạng báo cáo)](#-pattern-3-strategy-patternsstrategies)
     * [Observer Pattern (Subscribe sự kiện tự động)](#-pattern-4-observer-patternsobservers)
   * [3. Tầng Domain giàu tính đóng gói (Domain-Driven Invariants)](#3-tầng-domain-giàu-tính-đóng-gói-domain-driven-invariants)
   * [4. Composition Root tại `Program.cs` (Pure Dependency Injection)](#4-composition-root-tại-programcs-pure-dependency-injection)
   * [5. Hệ thống 21 Unit Test xUnit chuẩn AAA (`EduHub.Ch02.Tests/`)](#5-hệ-thống-21-unit-test-xunit-chuẩn-aaa-eduhubch02tests)
4. [Phần III: Bảng So Sánh Đối Chiếu Trước & Sau Refactor (Before vs. After)](#phần-iii-bảng-so-sánh-đối-chiếu-trước--sau-refactor-before-vs-after)
5. [Phần IV: Tra Cứu & Đối Chiếu 2 Đoạn Mã Trọng Tâm (Decorator & Factory)](#phần-iv-tra-cứu--đối-chiếu-2-đoạn-mã-trọng-tâm-decorator--factory)
   * [Đoạn 1: Mẫu Decorator (`LoggingClubRepository`)](#1-đoạn-mã-decorator-loggingclubrepository)
   * [Đoạn 2: Mẫu Factory Method (`NotificationServiceFactory`)](#2-đoạn-mã-factory-method-notificationservicefactory)
6. [Phần V: Kế Hoạch & Checklist Đạt Điểm Tuyệt Đối (Rubric 100 Điểm)](#phần-v-kế-hoạch--checklist-đạt-điểm-tuyệt-đối-rubric-100-điểm)

---

## 1. TỔNG QUAN GÓI TÀI LIỆU `05_Tai_Lieu`

Thư mục `05_Tai_Lieu` chứa gói bài tập Chương 2:
* Thư mục làm việc: `IN4211-ch02-solid-patterns-git/`
* Tệp tin nén lưu trữ: `IN4211-ch02-solid-patterns-git.zip` (dung lượng ~5.6 MB)

Bên trong gồm **2 solution C# (.NET 10)** độc lập:
1. **`starter/` (`Ch02.Refactor.sln`)**: Khởi điểm bài tập cho sinh viên. Mã nguồn cố tình viết dồn cục trong một "God Class" (`ClubManager`) vi phạm các nguyên lý SOLID, bỏ trống logic trong Domain và vô hiệu hóa các bài test xUnit.
2. **`solution/` (`EduHub.Ch02.sln`)**: Đáp án mẫu chuẩn mực của ThS. Nguyễn Tuấn Kiệt. Minh họa cách tái cấu trúc toàn diện từ God class sang kiến trúc phân tầng sạch, ứng dụng trọn vẹn 5 nguyên lý SOLID, 4 mẫu thiết kế GoF và hệ thống kiểm thử tự động xUnit (21 test xanh 100%).

---

## PHẦN I: PHÂN TÍCH MÃ NGUỒN KHỞI ĐIỂM (`starter/` — `Ch02.Refactor.sln`)

### 1. Hiện trạng God Class `ClubManager.cs` và các vi phạm SOLID

Lớp `ClubManager` là hiện thân của kiểu thiết kế "God Class" (Antipattern) ôm đồm toàn bộ trách nhiệm hệ thống:

```csharp
public class ClubManager
{
    private readonly List<Club> _clubs = new();
    private readonly List<Member> _members = new();
    private readonly string _logFilePath = "club-log.txt";
    // ...
}
```

#### Bảng chi tiết các vi phạm SOLID trong `ClubManager.cs`:

| Nguyên tắc bị vi phạm | Vị trí cụ thể trong code | Phân tích vi phạm kỹ thuật | Hậu quả kỹ thuật |
| :--- | :--- | :--- | :--- |
| **S — Single Responsibility** | `CreateClub()` (dòng 35–64)<br>`AddMemberToClub()` (dòng 67–84)<br>`CreateEvent()` (dòng 88–99)<br>`PrintClubReport()` (dòng 103–113) | Mỗi method gánh tới 4–5 trách nhiệm cùng lúc: vừa kiểm tra hợp lệ dữ liệu, vừa thêm vào danh sách, vừa ghi đĩa tệp tin log, vừa gửi thông báo qua console/email, vừa in báo cáo. | Class có quá nhiều lý do để thay đổi (Too many reasons to change). Sửa đổi định dạng email hoặc đổi nơi lưu file log có thể làm lỗi toàn bộ nghiệp vụ tạo CLB. |
| **O — Open/Closed** | `CreateClub()` (dòng 52–63)<br>`PrintClubReport()` (dòng 103–113) | Phân loại gửi thông báo bằng chuỗi `if/else` kiểm tra chuỗi: `presidentEmail.Contains("@gmail.com")`, `@yahoo.com`. Khi in báo cáo, code gắn cứng vào `Console.WriteLine`. | Mỗi khi nhà trường muốn hỗ trợ kênh mới (SMS, Slack, Zalo) hoặc muốn xuất báo cáo ra file CSV/PDF, lập trình viên buộc phải mở file `ClubManager.cs` ra sửa trực tiếp, vi phạm nguyên lý "Đóng với sửa đổi, Mở với mở rộng". |
| **D — Dependency Inversion** | Dòng 48, 79, 94, 111 (`File.AppendAllText`)<br>Dòng 54, 82, 97, 108 (`Console.WriteLine`) | Module cấp cao (`ClubManager`) gọi trực tiếp các API cấp thấp của hệ điều hành và I/O phần cứng (`File`, `Console`) mà không thông qua bất kỳ Abstraction (Interface) nào. | Khóa chặt mã nguồn vào hệ thống tệp đĩa thực tế. **Không thể viết Unit Test cô lập trong bộ nhớ (RAM)**. Mỗi lần chạy test sẽ tạo ra các file rác `club-log.txt` hoặc phụ thuộc quyền ghi ổ đĩa. |
| **I — Interface Segregation** | Cả lớp `ClubManager` (dòng 28–117) | Không khai báo interface, bắt các module gọi phải phụ thuộc vào một lớp cụ thể cồng kềnh với dữ liệu `List<Club>` và `List<Member>` nội tại. | Khó tích hợp các client chuyên biệt (ví dụ: máy Kiosk chỉ cần check-in nhưng phải phụ thuộc cả logic tạo CLB và in báo cáo). |

---

### 2. Hiện trạng Tầng Domain (`Club.cs`, `Event.cs`)
Trong `starter/`, tầng Domain bị rút sạch logic nghiệp vụ cốt lõi:
* `Domain/Club.cs`: Hằng số `MaxMembers = 50` và `MaxNameLength = 100` đã có, nhưng phương thức khởi tạo an toàn `Club.Create()` và hàm kiểm tra sức chứa `Club.HasRoom()` chỉ chứa:
  ```csharp
  throw new NotImplementedException();
  ```
* `Domain/Event.cs`: Hàm đăng ký tham dự `Event.Register(memberId)` và hàm điểm danh `Event.CheckIn(memberId)` cũng chỉ ném `throw new NotImplementedException()`.

---

### 3. Hiện trạng Tầng Kiểm Thử (`DomainTests.cs`)
Dự án kiểm thử `Ch02.Refactor.Tests/DomainTests.cs` chỉ có duy nhất 1 ca kiểm thử ban đầu:
* `CreateClub_WithEmptyName_ThrowsArgumentException`: Test kiểm tra tên rỗng trực tiếp trên `ClubManager`.
* 5 ca kiểm thử còn lại bị vô hiệu hóa với thuộc tính `[Fact(Skip = "TODO: ...")]`:
  1. `Club_Create_WithValidName_ReturnsClub`
  2. `Club_Create_WithInvalidName_ThrowsArgumentException`
  3. `Club_HasRoom_AtMaxMembers_ReturnsFalse`
  4. `Event_Register_NewMember_ReturnsTrue`
  5. `Event_CheckIn_UnregisteredMember_ThrowsInvalidOperationException`

---

## PHẦN II: PHÂN TÍCH MÃ NGUỒN ĐÁP ÁN CHUẨN (`solution/` — `EduHub.Ch02.sln`)

### 1. Cấu trúc tổng thể sau tái cấu trúc (Layered Clean Design)

Trong `solution/EduHub.Ch02`, giảng viên đã bẻ gãy God Class `ClubManager` thành kiến trúc phân tầng sạch với trách nhiệm đơn lẻ và phụ thuộc qua Interface:

```text
solution/EduHub.Ch02/
├── Domain/                   # Tầng thực thể miền nghiệp vụ (Entity & Invariants)
│   ├── Club.cs               # Tạo CLB an toàn, quản lý sĩ số
│   ├── Member.cs             # Định danh thành viên
│   └── Event.cs              # Quản lý đăng ký, điểm danh, tỉ lệ tham dự
├── Repositories/             # Tầng kho lưu trữ dữ liệu (Repository Pattern + Decorator)
│   ├── IClubRepository.cs / InMemoryClubRepository.cs
│   ├── IMemberRepository.cs / InMemoryMemberRepository.cs
│   ├── IEventRepository.cs / InMemoryEventRepository.cs
│   └── LoggingClubRepository.cs   <-- [DECORATOR PATTERN]
├── Logging/                  # Trừu tượng hóa ghi log (DIP)
│   ├── IAppLogger.cs
│   ├── ConsoleLogger.cs
│   └── FileLogger.cs
├── Notifications/            # Hệ thống phát thông báo (Factory Method Pattern)
│   ├── NotificationChannel.cs (Enum: Email, Sms, Slack)
│   ├── INotificationService.cs
│   ├── EmailNotificationService.cs
│   ├── SmsNotificationService.cs
│   ├── SlackNotificationService.cs
│   ├── INotificationServiceFactory.cs
│   └── NotificationServiceFactory.cs <-- [FACTORY PATTERN]
├── Patterns/
│   ├── Observers/            # Cơ chế phát thông báo sự kiện (Observer Pattern)
│   │   ├── IEventObserver.cs
│   │   ├── EmailNotifier.cs
│   │   └── LogNotifier.cs
│   └── Strategies/           # Đa dạng hóa định dạng báo cáo (Strategy Pattern)
│       ├── ClubStat.cs (Record DTO chứa số liệu)
│       ├── IReportStrategy.cs
│       ├── ConsoleReportStrategy.cs
│       └── CsvReportStrategy.cs
├── Services/                 # Tầng điều phối nghiệp vụ ứng dụng (SRP, DIP)
│   ├── IClubService.cs / ClubService.cs
│   ├── IEventService.cs / EventService.cs
│   └── IReportService.cs / ReportService.cs
└── Program.cs                # Composition Root (Nơi cấu hình Pure Dependency Injection)
```

---

### 2. Hiện thực hóa 4 Design Patterns kinh điển

#### 🏭 Pattern 1: Factory Method (`Notifications/`)
* **Mục tiêu:** Nơi phát thông báo chỉ cần ra lệnh theo kênh mong muốn (Email, SMS, Slack), không cần biết class cụ thể và không dùng từ khóa `new`.
* **Cấu trúc:**
  * Enum kênh: `public enum NotificationChannel { Email, Sms, Slack }`
  * Interface: `public interface INotificationServiceFactory { INotificationService Create(NotificationChannel channel); }`
  * Cài đặt: `NotificationServiceFactory` dùng biểu thức switch-case ánh xạ sang class cụ thể:
    ```csharp
    public INotificationService Create(NotificationChannel channel) => channel switch
    {
        NotificationChannel.Email => new EmailNotificationService(),
        NotificationChannel.Sms   => new SmsNotificationService(),
        NotificationChannel.Slack => new SlackNotificationService(),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, "Unsupported channel")
    };
    ```
* **Lợi ích:** Đạt OCP — Khi bổ sung kênh Zalo hay Telegram, chỉ cần tạo class mới và bổ sung 1 nhánh trong factory; các lớp service không hề bị sửa đổi.

---

#### 🎀 Pattern 2: Decorator (`LoggingClubRepository`)
* **Mục tiêu:** Bổ sung tính năng ghi log khi truy cập kho dữ liệu mà **không được sửa đổi `InMemoryClubRepository`** và không lạm dụng kế thừa class.
* **Cấu trúc:**
  * Lớp `LoggingClubRepository` cài đặt cùng interface `IClubRepository`.
  * Nhận một bản thể thực (`_inner`) và một `IAppLogger` thông qua constructor.
  * Khi gọi bất kỳ hành vi nào (như `Add`), nó ghi log trước rồi chuyển tiếp công việc cho `_inner`:
    ```csharp
    public void Add(Club club)
    {
        _logger.Info($"[repo] Add club '{club.Name}'");
        _inner.Add(club); // Ủy quyền cho bản repository thật bên trong
    }
    ```
* **Lợi ích:** Tính trong suốt (Transparency) — Tại `Program.cs`, nếu cần log thì bọc Decorator; nếu không cần log thì bỏ bọc, tầng `ClubService` hoàn toàn không nhận ra sự khác biệt.

---

#### 🎯 Pattern 3: Strategy (`Patterns/Strategies/`)
* **Mục tiêu:** Tách thuật toán định dạng và hiển thị báo cáo ra khỏi `ReportService`.
* **Cấu trúc:**
  * Định nghĩa dữ liệu đầu vào chung bằng Record:
    ```csharp
    public record ClubStat(string ClubName, int Members, int Events, int Registered, int CheckedIn);
    ```
  * Interface chiến lược:
    ```csharp
    public interface IReportStrategy
    {
        string Name { get; }
        string Render(IReadOnlyList<ClubStat> stats);
    }
    ```
  * Triển khai 2 chiến lược cụ thể:
    * `ConsoleReportStrategy`: Định dạng báo cáo dạng văn bản trực quan trên màn hình.
    * `CsvReportStrategy`: Định dạng bảng biểu CSV theo cột `Club,Members,Events,Registered,CheckedIn`.
  * `ReportService.Generate(IReportStrategy strategy)` chỉ việc tổng hợp dữ liệu thống kê rồi gọi `strategy.Render(stats)`.

---

#### 🔔 Pattern 4: Observer (`Patterns/Observers/`)
* **Mục tiêu:** Khi một sự kiện mới (`Event`) được tạo ra, hệ thống tự động thông báo đến tất cả các bên quan tâm theo cơ chế Publish-Subscribe (lỏng lẻo - Loose Coupling).
* **Cấu trúc:**
  * Interface người nghe:
    ```csharp
    public interface IEventObserver
    {
        void OnEventCreated(Event ev, Club club);
    }
    ```
  * Lớp phát tin (`EventService` - Subject): Duy trì danh sách `_observers` cùng các hàm `Subscribe()` / `Unsubscribe()`. Khi tạo xong sự kiện, duyệt danh sách để kích hoạt:
    ```csharp
    foreach (var observer in _observers.ToList())
        observer.OnEventCreated(ev, club);
    ```
  * Cài đặt 2 Observers độc lập:
    * `EmailNotifier`: Gửi email thông báo lịch sự kiện đến toàn bộ thành viên thuộc CLB.
    * `LogNotifier`: Ghi nhận sự kiện vào hệ thống ghi log (`IAppLogger`).

---

### 3. Tầng Domain giàu tính đóng gói (Domain-Driven Invariants)
Quy tắc nghiệp vụ được đưa về đúng vị trí bên trong thực thể Domain:
* **`Club.cs`:**
  * `Create(name, description)`: Kiểm tra toàn vẹn định danh (tên bắt buộc, không vượt quá 100 ký tự).
  * `HasRoom(currentMemberCount)`: Kiểm tra giới hạn sĩ số `currentMemberCount < MaxMembers (50)`.
* **`Event.cs`:**
  * `Register(memberId)`: Dùng `HashSet<Guid>` lưu trữ danh sách thành viên đăng ký. Tự động từ chối và trả về `false` nếu thành viên đăng ký trùng lặp.
  * `CheckIn(memberId)`: Bắt buộc thành viên phải có trong danh sách đăng ký (`RegisteredMemberIds`), nếu không sẽ ném ra ngoại lệ `InvalidOperationException("Member has not registered")`.
  * `AttendanceRate`: Thuộc tính tính toán tự động tỉ lệ điểm danh thực tế = `CheckedInMemberIds.Count / RegisteredMemberIds.Count`.

---

### 4. Composition Root tại `Program.cs` (Pure Dependency Injection)
[`Program.cs`](file:///G:/My%20Drive/Study/HK1_Nam_3/IN4211_Phat_Trien_Phan_Mem_Huong_Doi_Tuong/05_Tai_Lieu/IN4211-ch02-solid-patterns-git/IN4211-ch02-solid-patterns-git/solution/EduHub.Ch02/Program.cs) đóng vai trò là **Composition Root**:
* Nơi duy nhất trong giải pháp được phép dùng từ khóa `new` để tạo đối tượng cụ thể.
* Lắp ráp toàn bộ cây phụ thuộc:
  ```csharp
  IAppLogger logger = new ConsoleLogger();
  INotificationServiceFactory notificationFactory = new NotificationServiceFactory();
  INotificationService notifier = notificationFactory.Create(NotificationChannel.Email);

  IClubRepository clubRepo = new LoggingClubRepository(new InMemoryClubRepository(), logger); // Decorator
  IMemberRepository memberRepo = new InMemoryMemberRepository();
  IEventRepository eventRepo = new InMemoryEventRepository();

  IClubService clubService = new ClubService(clubRepo, memberRepo, notifier, logger);
  IEventService eventService = new EventService(clubRepo, memberRepo, eventRepo);
  IReportService reportService = new ReportService(clubRepo, memberRepo, eventRepo);
  ```
* Chạy kịch bản mẫu 5 khối: Tạo CLB ➜ Observer kích hoạt ➜ Xuất báo cáo Console & CSV (Strategy) ➜ Đổi kênh thông báo (Factory) ➜ Kiểm tra ngoại lệ validate.

---

### 5. Hệ thống 21 Unit Test xUnit chuẩn AAA (`EduHub.Ch02.Tests/`)
Dự án test đạt tỷ lệ vượt qua **21/21 tests xanh (100%)**, tuân thủ cấu trúc **Arrange - Act - Assert (AAA)**:
1. **`ClubTests.cs` (4 tests):**
   * Tạo CLB hợp lệ có ID GUID và tên đúng.
   * `[Theory]` với `[InlineData("", ...)]`, `[InlineData("   ", ...)]` kiểm tra ném `ArgumentException`.
   * Tên quá 100 ký tự ném `ArgumentException`.
   * `[Theory]` kiểm tra các giá trị biên của `HasRoom`: 0 (true), 49 (true), 50 (false).
2. **`EventTests.cs` (6 tests):**
   * Đăng ký thành viên mới thành công (trả về `true`).
   * Đăng ký trùng lặp thành viên trả về `false` và giữ nguyên 1 bản ghi.
   * Check-in cho thành viên đã đăng ký thành công.
   * Check-in cho thành viên chưa đăng ký ném `InvalidOperationException`.
   * Kiểm tra tỉ lệ `AttendanceRate`: bằng 0 khi chưa ai đăng ký, bằng 0.5 khi 1 trong 2 người check-in.
3. **`PatternTests.cs` (11 tests):**
   * Factory: Kiểm tra trả về đúng kiểu Service tương ứng từng kênh enum.
   * Decorator: Dùng `FakeLogger` xác minh vừa ghi log vừa lưu vào repository bên trong.
   * Strategy: Cùng dữ liệu `ClubStat` kết xuất ra đúng định dạng Console và CSV.
   * Observer: Dùng `SpyObserver` chứng minh `EventService` tự động thông báo đến tất cả các bên đăng ký khi có sự kiện mới.

---

## PHẦN III: BẢNG SO SÁNH ĐỐI CHIẾU TRƯỚC & SAU REFACTOR (BEFORE vs. AFTER)

| Hạng mục so sánh | Bản khởi điểm (`starter/`) | Bản đáp án chuẩn (`solution/`) |
| :--- | :--- | :--- |
| **Mô hình kiến trúc** | **God Class** (`ClubManager` ôm trọn 5 trách nhiệm) | **Layered Clean Architecture** phân tách rõ rệt giữa Domain, Service, Repository, Pattern |
| **Quy tắc nghiệp vụ** | Trôi dạt, nằm lẫn lộn trong `ClubManager` | Đóng gói chặt chẽ trong Domain Entities (`Club`, `Event`) |
| **Phát thông báo** | Chuỗi `if/else` kiểm tra đuôi chuỗi email | **Factory Method Pattern** (`NotificationServiceFactory`) |
| **Ghi vết hệ thống** | Gọi trực tiếp `File.AppendAllText` ra đĩa cứng | **IAppLogger (DIP)** + **Decorator Pattern** (`LoggingClubRepository`) |
| **Báo cáo thống kê** | Hardcode `Console.WriteLine` | **Strategy Pattern** (`ConsoleReportStrategy`, `CsvReportStrategy`) |
| **Bắn sự kiện mới** | Vòng lặp foreach gửi email thủ công | **Observer Pattern** (`EventService.Subscribe` + `IEventObserver`) |
| **Lưu trữ dữ liệu** | Phụ thuộc danh sách `List<T>` trong bộ nhớ nội bộ | Phân tách Repository (`IClubRepository`, `IMemberRepository`, `IEventRepository`) |
| **Unit Test** | 1 test chạy được, 5 test bị `Skip` | **21 tests AAA hoàn chỉnh**, bao phủ Domain, Exceptions và 4 Design Patterns |

---

## PHẦN IV: TRA CỨU & ĐỐI CHIẾU 2 ĐOẠN MÃ TRỌNG TÂM (DECORATOR & FACTORY)

Dưới đây là vị trí chính xác của 2 đoạn mã được trích dẫn trong câu hỏi:

### 1. Đoạn mã Decorator (`LoggingClubRepository`)

```csharp
public class LoggingClubRepository : IClubRepository {
 private readonly IClubRepository _inner; // repository thật bên trong
 private readonly IAppLogger _logger;
 /* constructor nhận _inner và _logger */
 public void Add(Club club) {
 _logger.Info($"[repo] Add club '{club.Name}'"); // việc thêm vào
 _inner.Add(club); // giao cho bản thật
 }
 /* GetAll, GetById: ghi log rồi giao lại tương tự */
}
```

* **Vị trí trong Slide bài giảng:** **Slide 30 / 72** — Đề mục: *Đoạn mã 2.5: Decorator ghi log cho kho dữ liệu (rút gọn từ đáp án mẫu)*.
  * Tệp Markdown: [`02_Bai_Giang/IN4211-ch02-solid-patterns-git.md` (Dòng 429–443)](file:///G:/My%20Drive/Study/HK1_Nam_3/IN4211_Phat_Trien_Phan_Mem_Huong_Doi_Tuong/02_Bai_Giang/IN4211-ch02-solid-patterns-git.md#L429-L443)
  * Tệp Slide PDF: `02_Bai_Giang/IN4211-ch02-solid-patterns-git.pdf`
* **Vị trí trong Solution mã nguồn:** Tệp [`Repositories/LoggingClubRepository.cs`](file:///G:/My%20Drive/Study/HK1_Nam_3/IN4211_Phat_Trien_Phan_Mem_Huong_Doi_Tuong/05_Tai_Lieu/IN4211-ch02-solid-patterns-git/IN4211-ch02-solid-patterns-git/solution/EduHub.Ch02/Repositories/LoggingClubRepository.cs#L10-L38)
* **Ý nghĩa:** Giải quyết bài toán bổ sung tính năng logging cho kho dữ liệu mà không sửa đổi lớp gốc `InMemoryClubRepository`.

---

### 2. Đoạn mã Factory Method (`NotificationServiceFactory`)

```csharp
public enum NotificationChannel { Email, Sms, Slack }
public interface INotificationServiceFactory {
 INotificationService Create(NotificationChannel channel);
}
public class NotificationServiceFactory : INotificationServiceFactory {
 public INotificationService Create(NotificationChannel c) => c switch {
 NotificationChannel.Email => new EmailNotificationService(),
 NotificationChannel.Sms => new SmsNotificationService(),
 NotificationChannel.Slack => new SlackNotificationService(),
 _ => throw new ArgumentOutOfRangeException(nameof(c))
 }
```

* **Vị trí trong Slide bài giảng:** **Slide 27 / 72** — Đề mục: *Đoạn mã 2.3: Factory tạo kênh thông báo (rút gọn từ đáp án mẫu)*.
  * Tệp Markdown: [`02_Bai_Giang/IN4211-ch02-solid-patterns-git.md` (Dòng 390–403)](file:///G:/My%20Drive/Study/HK1_Nam_3/IN4211_Phat_Trien_Phan_Mem_Huong_Doi_Tuong/02_Bai_Giang/IN4211-ch02-solid-patterns-git.md#L390-L403)
  * Tệp Slide PDF: `02_Bai_Giang/IN4211-ch02-solid-patterns-git.pdf`
* **Vị trí trong Solution mã nguồn:** Được tổ chức thành 3 tệp tin chuyên biệt:
  * Enum kênh: [`Notifications/NotificationChannel.cs`](file:///G:/My%20Drive/Study/HK1_Nam_3/IN4211_Phat_Trien_Phan_Mem_Huong_Doi_Tuong/05_Tai_Lieu/IN4211-ch02-solid-patterns-git/IN4211-ch02-solid-patterns-git/solution/EduHub.Ch02/Notifications/NotificationChannel.cs#L3-L8)
  * Interface nhà máy: [`Notifications/INotificationServiceFactory.cs`](file:///G:/My%20Drive/Study/HK1_Nam_3/IN4211_Phat_Trien_Phan_Mem_Huong_Doi_Tuong/05_Tai_Lieu/IN4211-ch02-solid-patterns-git/IN4211-ch02-solid-patterns-git/solution/EduHub.Ch02/Notifications/INotificationServiceFactory.cs#L4-L7)
  * Cài đặt nhà máy: [`Notifications/NotificationServiceFactory.cs`](file:///G:/My%20Drive/Study/HK1_Nam_3/IN4211_Phat_Trien_Phan_Mem_Huong_Doi_Tuong/05_Tai_Lieu/IN4211-ch02-solid-patterns-git/IN4211-ch02-solid-patterns-git/solution/EduHub.Ch02/Notifications/NotificationServiceFactory.cs#L7-L16)
* **Ý nghĩa:** Thay thế toàn bộ chuỗi `if/else` kiểm tra đuôi email cứng trong `ClubManager` cũ, giúp hệ thống đạt chuẩn Open/Closed Principle (OCP).

---

## PHẦN V: KẾ HOẠCH & CHECKLIST ĐẠT ĐIỂM TUYỆT ĐỐI (RUBRIC 100 ĐIỂM)

Căn cứ Slide 68 của bài giảng, thang điểm đánh giá Bài tập Chương 2 (HĐ1.3) gồm 5 tiêu chí:

| Nhiệm vụ | Yêu cầu kỹ thuật | Trọng số | Trạng thái đạt được |
| :--- | :--- | :---: | :---: |
| **1. Phân tích vi phạm SOLID** | Lập tài liệu `docs/solid-analysis.md` chỉ rõ ≥ 4 vi phạm SOLID trong `ClubManager.cs` (nêu rõ số dòng, nguyên tắc S-O-D-I và hậu quả kỹ thuật). | **15 điểm** | ✅ Đã phân tích chi tiết tại Phần I |
| **2. Tách lớp & Interface** | Tách `ClubManager` thành ≥ 4 class mới (`ClubService`, `EventService`, `ReportService`, `IAppLogger`, các Repository). Mọi phụ thuộc bắt buộc qua Interface. | **30 điểm** | ✅ Đã cấu trúc mẫu tại Phần II |
| **3. Cài đặt 4 Design Patterns** | Triển khai chính xác 4 mẫu thiết kế: **Factory** (thông báo), **Decorator** (logging repo), **Strategy** (báo cáo), **Observer** (báo sự kiện). | **25 điểm** | ✅ Đã phân tích chi tiết tại Phần II.2 |
| **4. Unit Test với xUnit (AAA)** | Viết ≥ 6 unit test xUnit cho tầng Domain (`Club`, `Event`), tuân thủ Arrange-Act-Assert, có kiểm tra ngoại lệ `Assert.Throws` và kiểm tra giá trị biên `[Theory]`. | **15 điểm** | ✅ Solution mẫu có 21 test xanh 100% |
| **5. Quy trình Git & Pull Request** | Làm trên nhánh `feature/solid-refactor`, ≥ 5 commit Conventional Commits, thực hiện `git rebase main`, mở Pull Request chuẩn What/Why/How kèm ≥ 2 nhận xét review. | **15 điểm** | ✅ Đã có kịch bản triển khai chuẩn |
| **TỔNG CỘNG** | | **100 điểm** | 🏆 **MỤC TIÊU ĐẠT ĐIỂM TỐI ĐA** |

---
*Báo cáo được biên soạn và chuẩn hóa phục vụ học phần IN4211 — Phát triển phần mềm hướng đối tượng.*
