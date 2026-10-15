# 🎓 IN4221 — PHÁT TRIỂN PHẦN MỀM HƯỚNG ĐỐI TƯỢNG (OOP)
## KHOA CÔNG NGHỆ VÀ KỸ THUẬT — TRƯỜNG ĐẠI HỌC ĐỒNG THÁP

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![C# 13](https://img.shields.io/badge/C%23-13.0-239120?style=flat&logo=c-sharp)](https://docs.microsoft.com/dotnet/csharp/)
[![OOP Architecture](https://img.shields.io/badge/Architecture-Clean%20OOP%20%7C%20SOLID-blue?style=flat)]()
[![Git Workflow](https://img.shields.io/badge/Git-Conventional%20Commits-orange?style=flat)]()

---

## 📌 THÔNG TIN HỌC PHẦN & SINH VIÊN

* **Học phần:** Phát triển phần mềm hướng đối tượng (Mã lớp học phần: **IN4221** / IN421104)
* **Giảng viên hướng dẫn:** **ThS. Nguyễn Tuấn Kiệt** (Email: `nguyentuankiet@dthu.edu.vn`)
* **Sinh viên thực hiện:** **Phan Trần Gia Hưng**
* **Mã số sinh viên (MSSV):** **`0024417689`**
* **Lớp sinh hoạt:** **DHCNTT24B**
* **Kho lưu trữ chính thức (GitHub):** [giahung25/IN4221_PTPMHDT_Phan_Tran_Gia_Hung](https://github.com/giahung25/IN4221_PTPMHDT_Phan_Tran_Gia_Hung)

---

## 🗂️ MỤC LỤC BÀI TẬP CÁC CHƯƠNG (COURSE MODULES)

Kho mã nguồn được tổ chức theo từng chương bài học độc lập, rõ ràng và chuẩn mực:

```text
IN4221_PTPMHDT/
├── README.md                           # Master README tổng quan toàn bộ môn học (tệp này)
├── .gitignore                          # Quy chuẩn loại trừ file rác (.NET, IDE, OS)
│
├── ch01/                               # 📁 CHƯƠNG 01: PHÂN TÍCH YÊU CẦU & DOMAIN MODEL
│   ├── EduHub.sln                      # Solution C# (.NET 10)
│   ├── EduHub.Domain/                  # 5 Thực thể miền: Club, Member, Event, Registration, CheckIn
│   ├── EduHub.Console/                 # Chương trình Console chạy 8 bước nghiệp vụ & test Invariants
│   ├── README.md                       # Báo cáo chi tiết Chương 1 & Sơ đồ lớp Mermaid (Hình 1)
│   └── docs/                           # Tài liệu đính kèm (Word User Stories & Quy chuẩn coding)
│
├── ch02/                               # 📁 CHƯƠNG 02: SOLID, DESIGN PATTERNS & XUNIT TESTING
│   ├── Ch02.Refactor.sln               # Solution C# (.NET 10)
│   ├── Ch02.Refactor/                  # Dự án Refactoring: Tách God Class ClubManager
│   ├── Ch02.Refactor.Tests/            # Dự án Unit Test tự động với xUnit (Arrange-Act-Assert)
│   ├── README.md                       # Hướng dẫn chi tiết nhiệm vụ bài tập Chương 2
│   └── docs/                           # Báo cáo phân tích vi phạm SOLID & đối chiếu mẫu thiết kế
│       ├── solid-analysis.md           # Phân tích ≥ 4 vi phạm SOLID trong ClubManager
│       └── PHAN_TICH_MA_NGUON_CH02_SOLID_PATTERNS.md
│
└── ch03/ ... (Sẵn sàng mở rộng cho các chương tiếp theo)
```

---

## 📖 TÓM TẮT NỘI DUNG TỪNG CHƯƠNG

### 🔹 [Chương 01 — Xây Dựng Bộ User Stories & Mô Hình Hóa Thực Thể EduHub](./ch01/)
* **Chủ đề:** Phân tích yêu cầu theo Slide trang 34 — Thiết kế Domain Entities với tính đóng gói nghiêm ngặt.
* **Kết quả đạt được:**
  * Xây dựng bộ **6 User Stories** độc lập (bao phủ 3 tác nhân: Sinh viên, Ban CLB, Quản trị viên) kèm tiêu chí nghiệm thu **Acceptance Criteria (Given - When - Then)**.
  * Hiện thực hóa 5 Entity C# (.NET 10) tuân thủ tính đóng gói: `{ get; private set; }`, Constructor `private`, Factory Method `Create()`, bảo vệ tập hợp bằng `IReadOnlyList<T>`.
  * 🖼️ **Hình 1 — Sơ đồ lớp hướng đối tượng (Mermaid Class Diagram)** hiển thị trực quan quan hệ giữa các thực thể.
  * Kịch bản Console mô phỏng 8 bước vòng đời CLB/Sự kiện và 4 ca kiểm thử ngoại lệ Invariants.
* 📄 **Chi tiết báo cáo Chương 1:** Xem tại [`ch01/README.md`](./ch01/README.md).
* ⚙️ **Lệnh chạy:**
  ```bash
  dotnet build ch01/EduHub.sln
  dotnet run --project ch01/EduHub.Console
  ```

---

### 🔹 [Chương 02 — Tái Cấu Trúc (Refactoring) Với SOLID, Design Patterns & xUnit](./ch02/)
* **Chủ đề:** Xóa bỏ tư duy "God Class" (`ClubManager`), áp dụng 5 nguyên lý SOLID, 4 mẫu thiết kế GoF và kiểm thử tự động.
* **Kết quả đạt được:**
  * **Nhiệm vụ 1:** Báo cáo phân tích chi tiết ≥ 4 vi phạm SOLID (SRP, OCP, DIP, ISP) tại [`ch02/docs/solid-analysis.md`](./ch02/docs/solid-analysis.md).
  * **Nhiệm vụ 2:** Phân rã God Class thành các Service (`ClubService`, `EventService`, `ReportService`) và Repository độc lập qua Interface.
  * **Nhiệm vụ 3:** Cài đặt 4 Design Patterns kinh điển:
    * 🏭 **Factory Method:** `NotificationServiceFactory` quản lý kênh gửi thông báo (Email, SMS, Slack).
    * 🎀 **Decorator:** `LoggingClubRepository` bọc ngoài repository ghi log truy cập trong suốt.
    * 🎯 **Strategy:** `IReportStrategy` kết xuất báo cáo linh hoạt ra Console và CSV.
    * 🔔 **Observer:** `IEventObserver` kích hoạt phát thông báo tự động khi tạo sự kiện.
  * **Nhiệm vụ 4:** Hoàn thiện tầng Domain (`Club`, `Event`) và viết hệ thống Unit Test xUnit theo mẫu **Arrange - Act - Assert (AAA)** kiểm tra giá trị biên và ngoại lệ.
  * **Nhiệm vụ 5:** Vận hành quy trình Git nâng cao trên nhánh `feature/solid-refactor`, rebase lên `main` và mở Pull Request chuyên nghiệp.
* 📄 **Chi tiết hướng dẫn Chương 2:** Xem tại [`ch02/README.md`](./ch02/README.md).
* ⚙️ **Lệnh chạy:**
  ```bash
  dotnet build ch02/Ch02.Refactor.sln
  dotnet test ch02/Ch02.Refactor.sln
  dotnet run --project ch02/Ch02.Refactor
  ```

---

## 🛠️ YÊU CẦU MÔI TRƯỜNG & CÔNG CỤ
* **Hệ điều hành:** Windows 10/11, Linux, macOS
* **SDK:** [.NET SDK 10.0](https://dotnet.microsoft.com/) (hoặc .NET 8.0/9.0)
* **IDE khuyên dùng:** Visual Studio 2022 / VS Code / JetBrains Rider
* **Hệ thống quản lý phiên bản:** Git (tuân thủ chuẩn Conventional Commits)

---
*© 2026 Phan Trần Gia Hưng — MSSV: 0024417689 — DHCNTT24B — Trường Đại học Đồng Tháp*
