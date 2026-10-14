# 📜 QUY CHUẨN LẬP TRÌNH BẮT BUỘC — HỌC PHẦN IN4211 (EDUHUB)
## PHÁT TRIỂN PHẦN MỀM HƯỚNG ĐỐI TƯỢNG · THS. NGUYỄN TUẤN KIỆT
> **MỤC TIÊU:** Tài liệu này là kim chỉ nam bắt buộc (Strict Coding Rules) áp dụng cho mọi đoạn code, bài tập chương và đồ án lớn EduHub (.NET 10). Mọi vi phạm các quy chuẩn dưới đây đều bị **TRỪ ĐIỂM TRỰC TIẾP** theo barem đánh giá của giảng viên.

---

## 🚫 1. BẢNG ĐỐI CHIẾU LỖI NGUY HIỂM & CÁCH SỬA CHUẨN (DO'S & DON'TS)

| Vi phạm (BỊ TRỪ ĐIỂM NẶNG ❌) | Quy chuẩn bắt buộc (ĐẠT ĐIỂM TỐI ĐA ✅) | Giải thích lý do |
| :--- | :--- | :--- |
| **`public set` trên thuộc tính**<br>`public string Name { get; set; }` | **`{ get; private set; }`**<br>`public string Name { get; private set; }` | Vi phạm tính đóng gói (Encapsulation). Bên ngoài có thể can thiệp làm sai lệch dữ liệu nội tại của entity. |
| **Dùng Constructor `public`**<br>`var m = new Member("Hưng", ...);` | **Constructor `private` + Factory `Create()`**<br>`var m = Member.Create("Hưng", ...);` | Cho phép `new` tự do sẽ tạo ra các đối tượng rác không được kiểm tra tính hợp lệ nghiệp vụ (Invariants). |
| **Public `List<T>` công khai**<br>`public List<Member> Members { get; set; }` | **`IReadOnlyList<T>` + `AddMember()`**<br>`public IReadOnlyList<Member> Members => _members.AsReadOnly();` | Public List cho phép bên ngoài tự ý `.Add()` / `.Clear()` mà không qua kiểm tra trùng lặp hoặc luật nghiệp vụ của CLB. |
| **Dùng String tự do cho trạng thái**<br>`if (status == "Pending")` | **Dùng `Enum` tường minh**<br>`public RegistrationStatus Status { get; private set; }` | Chuỗi tự do dễ sai chính tả, khó bảo trì, phân tán logic và không tận dụng được kiểm tra kiểu tĩnh của C#. |
| **Thiếu khóa ngoại (FK)**<br>`Event` chỉ có `Title`, thiếu `ClubId` | **Phía "Nhiều" bắt buộc giữ `Guid` FK**<br>`public Guid ClubId { get; private set; }` | Không thể ánh xạ quan hệ trong Domain Model và không thể tương thích với EF Core / Database sau này. |
| **Commit chung chung trên Git**<br>`git commit -m "update code"` | **Chuẩn Conventional Commits**<br>`git commit -m "feat(domain): add Club and Member entities"` | Quy định chấm điểm HĐ1.3 và HĐ2.1 bắt buộc commit có cấu trúc rõ ràng. |

---

## 🏛️ 2. QUY TẮC THIẾT KẾ DOMAIN ENTITIES (OOP DESIGN RULES)

### Quy tắc 2.1: Tính đóng gói thuộc tính (Encapsulation)
* Tất cả thuộc tính của Entity **BẮT BUỘC** khai báo theo dạng:
  ```csharp
  public Guid Id { get; private set; }
  public string Name { get; private set; } = string.Empty;
  ```
* Muốn thay đổi trạng thái hoặc dữ liệu của Entity, **BẮT BUỘC** phải gọi phương thức hành vi nội tại:
  ```csharp
  // SAI ❌:
  student.Status = MemberStatus.Inactive;

  // ĐÚNG ✅:
  student.Deactivate();
  ```

---

### Quy tắc 2.2: Khởi tạo đối tượng qua Factory Method `Create()`
* **Constructor BẮT BUỘC phải là `private`**:
  ```csharp
  private Member(string name, string email, string studentCode)
  {
      Id = Guid.NewGuid();
      Name = name;
      Email = email;
      StudentCode = studentCode;
      JoinedAt = DateTime.UtcNow;
      Status = MemberStatus.Active;
  }
  ```
* **Mọi đối tượng chỉ được sinh ra qua phương thức tĩnh `Create(...)`**:
  ```csharp
  public static Member Create(string name, string email, string studentCode)
  {
      // 1. Kiểm tra toàn bộ điều kiện bất biến (Invariants Validation)
      if (string.IsNullOrWhiteSpace(name))
          throw new ArgumentException("Tên thành viên không được để trống.", nameof(name));
      
      if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
          throw new ArgumentException("Email không đúng định dạng.", nameof(email));
          
      if (string.IsNullOrWhiteSpace(studentCode))
          throw new ArgumentException("Mã số sinh viên không được để trống.", nameof(studentCode));

      // 2. Chuẩn hóa dữ liệu trước khi gán
      return new Member(name.Trim(), email.Trim().ToLowerInvariant(), studentCode.Trim().ToUpperInvariant());
  }
  ```

---

### Quy tắc 2.3: Bảo vệ và đóng gói tập hợp (Encapsulating Collections)
* Khi một Entity chứa danh sách con (ví dụ: `Club` chứa danh sách `Member`):
  ```csharp
  public class Club
  {
      // Biến lưu trữ nội bộ (private field)
      private readonly List<Member> _members = new();

      // Chỉ công khai ra ngoài dưới dạng CHỈ ĐỌC (IReadOnlyList)
      public IReadOnlyList<Member> Members => _members.AsReadOnly();

      // Mọi thao tác thêm phần tử phải đi qua hàm hành vi có thẩm định nghiệp vụ
      public void AddMember(Member member)
      {
          ArgumentNullException.ThrowIfNull(member);

          // Kiểm tra trùng lặp ID hoặc MSSV
          if (_members.Any(m => m.Id == member.Id || m.StudentCode == member.StudentCode))
              throw new InvalidOperationException($"Sinh viên '{member.Name}' ({member.StudentCode}) đã là thành viên của CLB.");

          _members.Add(member);
      }

      public int MemberCount() => _members.Count;
  }
  ```

---

### Quy tắc 2.4: Quản lý vòng đời trạng thái bằng Enum
* Trạng thái phiếu đăng ký (`RegistrationStatus`) có vòng đời chuyển dịch nghiêm ngặt:
  - `Pending` (Mới đăng ký, chờ duyệt)
  - `Confirmed` (Đã được ban tổ chức xác nhận)
  - `Cancelled` (Đã hủy)
* **Phương thức chuyển trạng thái phải kiểm tra điều kiện tiên quyết:**
  ```csharp
  public void Confirm()
  {
      // Chỉ phiếu Pending mới được chuyển sang Confirmed
      if (Status != RegistrationStatus.Pending)
          throw new InvalidOperationException($"Không thể xác nhận phiếu đang ở trạng thái {Status}.");

      Status = RegistrationStatus.Confirmed;
  }

  public void Cancel()
  {
      if (Status == RegistrationStatus.Cancelled)
          throw new InvalidOperationException("Phiếu đăng ký này đã bị hủy trước đó.");

      Status = RegistrationStatus.Cancelled;
  }
  ```
* **Chỉ cho phép Check-in khi phiếu đã `Confirmed`:**
  ```csharp
  public static CheckIn Create(Registration registration, CheckInMethod method = CheckInMethod.QRCode)
  {
      ArgumentNullException.ThrowIfNull(registration);

      if (registration.Status != RegistrationStatus.Confirmed)
          throw new InvalidOperationException($"Chỉ phiếu đăng ký đã xác nhận (Confirmed) mới được phép check-in. Hiện tại: {registration.Status}.");

      return new CheckIn(registration.Id, method);
  }
  ```

---

### Quy tắc 2.5: Quy chuẩn Khóa chính và Khóa ngoại (PK & FK)
* **Khóa chính (PK):** Toàn bộ Entity trong EduHub sử dụng kiểu **`Guid`** (`Id = Guid.NewGuid();`). Tuyệt đối không dùng `int` tự tăng trong Domain Layer.
* **Khóa ngoại (FK):** Phía "Nhiều" (`*`) bắt buộc phải khai báo trường `Guid` tham chiếu tới phía "Một" (`1`):
  - `Event` bắt buộc có `ClubId` (`Guid`).
  - `Registration` bắt buộc có `MemberId` (`Guid`) và `EventId` (`Guid`).
  - `CheckIn` bắt buộc có `RegistrationId` (`Guid`).

---

## 🌿 3. QUY TRÌNH GIT & COMMIT MESSAGE CHUẨN QUỐC TẾ

### Quy tắc 3.1: Quản lý nhánh (Branching)
* Tuyệt đối **KHÔNG commit thẳng vào nhánh `main`**.
* Mỗi bài tập / tính năng phải tạo một nhánh riêng theo cú pháp:
  - Bài tập Chương 1: `ch01/<MSSV>` (Ví dụ: `ch01/0024417689`)
  - Bài tập Chương 2: `ch02/<MSSV>`
  - Tính năng BTL: `feature/<tên-tính-năng>`

### Quy tắc 3.2: Quy ước Commit Message (Conventional Commits)
Cú pháp bắt buộc:  
👉 `<type>(<scope>): <mô tả ngắn bằng tiếng Anh hoặc tiếng Việt không dấu>`

Các tiền tố `type` hợp lệ:
* **`feat`**: Thêm tính năng / entity / phương thức mới.  
  *Ví dụ:* `feat(domain): add Member and Club entities with invariants validation`
* **`fix`**: Sửa lỗi logic hoặc ngoại lệ.  
  *Ví dụ:* `fix(event): prevent event creation with negative capacity`
* **`docs`**: Cập nhật tài liệu, README, sơ đồ lớp.  
  *Ví dụ:* `docs(ch01): add Mermaid class diagram and user stories`
* **`test`**: Bổ sung unit test hoặc kịch bản kiểm thử trong `Program.cs`.  
  *Ví dụ:* `test(console): add edge-case validation tests for registration flow`
* **`refactor`**: Tái cấu trúc mã nguồn mà không làm đổi hành vi.

### Quy tắc 3.3: Mẫu mô tả khi mở Pull Request (PR Template)
Mỗi Pull Request mở về `main` bắt buộc phải có nội dung tối thiểu 3 phần:
```markdown
## What (Thay đổi những gì?)
- Tạo 5 Domain Entity cho EduHub: Member, Club, Event, Registration, CheckIn.
- Viết chương trình Console demo 8 bước chạy luồng nghiệp vụ hoàn chỉnh.

## Why (Tại sao làm việc này?)
- Hoàn thành bài tập Chương 1 (HĐ1.3) đáp ứng chuẩn đầu ra CLO1, CLO4, CLO5.

## How to test (Cách kiểm thử?)
- Chạy lệnh: `dotnet run --project EduHub.Console`
- Quan sát kết quả in ra màn hình: 7 bước nghiệp vụ chạy thành công và 3 bước thử nghiệm vi phạm luật nghiệp vụ được bắt lỗi chính xác.
```

---

## 🎯 4. CHECKLIST TỰ RÀ SOÁT TRƯỚC KHI NỘP BÀI (PRE-SUBMISSION CHECKLIST)

Trước khi mở Pull Request để chấm điểm, sinh viên phải tự đánh dấu kiểm tra đủ 6 mục sau:
- [ ] Không có bất kỳ thuộc tính nào để `public set`.
- [ ] Mọi Entity đều có Constructor `private` và phương thức khởi tạo tĩnh `Create()`.
- [ ] Mọi trường hợp dữ liệu rỗng/sai định dạng đều ném ra đúng loại ngoại lệ (`ArgumentException`, `ArgumentOutOfRangeException`, `InvalidOperationException`).
- [ ] Toàn bộ danh sách con đều được bao bọc bởi `IReadOnlyList<T>`.
- [ ] Lệnh `dotnet run` thực thi trơn tru, không có lỗi biên dịch (0 warning, 0 error).
- [ ] Nhánh Git chứa ít nhất 3 commit có message đúng chuẩn Conventional Commits.
