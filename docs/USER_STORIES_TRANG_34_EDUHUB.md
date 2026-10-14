# 📝 BÀI TẬP TRANG 34: XÂY DỰNG BỘ USER STORIES CHO DỰ ÁN EDUHUB
## HỌC PHẦN: PHÁT TRIỂN PHẦN MỀM HƯỚNG ĐỐI TƯỢNG (IN4211)
**Giảng viên:** ThS. Nguyễn Tuấn Kiệt · **Trường Đại học Đồng Tháp**  
**Yêu cầu đề bài (Slide trang 34):**  
> *"Tự viết ít nhất 5 story, đủ 3 vai trò. Đừng chép story mẫu!"*

---

## 🎯 DANH SÁCH 6 USER STORIES HOÀN CHỈNH (KHÔNG TRÙNG MẪU)

| Mã Story | Tác nhân (Actor) | Tên tính năng | Tóm tắt nghiệp vụ |
| :---: | :--- | :--- | :--- |
| **US-NEW-01** | 👤 Sinh viên (Member) | Hủy phiếu đăng ký sự kiện | Sinh viên bận đột xuất muốn hủy phiếu trước giờ G để nhường chỗ |
| **US-NEW-02** | 👤 Sinh viên (Member) | Rút tên khỏi Câu lạc bộ | Sinh viên tự nguyện rời CLB khi không còn nhu cầu sinh hoạt |
| **US-NEW-03** | 🧑‍💼 Ban chủ nhiệm CLB | Hủy bỏ sự kiện đã lên lịch | Ban tổ chức hủy sự kiện do trường hợp bất khả kháng |
| **US-NEW-04** | 🧑‍💼 Ban chủ nhiệm CLB | Duyệt hàng loạt phiếu đăng ký | Duyệt các phiếu Pending trong giới hạn sức chứa (Capacity) còn lại |
| **US-NEW-05** | 🛠 Quản trị viên (Admin) | Phê duyệt thành lập CLB mới | Khởi tạo tổ chức CLB mới với kiểm tra ràng buộc định danh |
| **US-NEW-06** | 🛠 Quản trị viên (Admin) | Khôi phục trạng thái thành viên | Mở lại quyền hoạt động (Reactivate) cho thành viên từng bị Inactive |

---

## 📋 CHI TIẾT TỪNG USER STORY & TIÊU CHÍ NGHIỆM THU (ACCEPTANCE CRITERIA)

### 👤 NHÓM 1: TÁC NHÂN SINH VIÊN (MEMBER)

#### 🔹 US-NEW-01: Hủy phiếu đăng ký sự kiện (Cancel Event Registration)
* **User Story:**
  > *"Là một **Sinh viên**, tôi muốn **hủy phiếu đăng ký tham gia sự kiện** của mình trước khi sự kiện bắt đầu, để **nhường lại cơ hội và suất tham dự cho các bạn sinh viên khác**."*

* **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **AC-01.1 (Hủy thành công khi phiếu chưa điểm danh):**
    - **Given:** Sinh viên sở hữu một phiếu đăng ký đang ở trạng thái `Pending` hoặc `Confirmed`.
    - **When:** Sinh viên thực hiện hành động hủy phiếu qua phương thức `registration.Cancel()`.
    - **Then:** Thuộc tính `Status` chuyển sang `RegistrationStatus.Cancelled`, đồng thời số lượng chỗ khả dụng của sự kiện được hoàn trả (+1 chỗ trống).
  - **AC-01.2 (Chặn hủy khi đã Check-in hoặc phiếu đã bị hủy trước đó):**
    - **Given:** Phiếu đăng ký đã được xác nhận điểm danh (`CheckIn`) thành công hoặc vốn đã ở trạng thái `Cancelled`.
    - **When:** Sinh viên cố gắng gửi yêu cầu hủy phiếu.
    - **Then:** Hệ thống từ chối và ném ra ngoại lệ `InvalidOperationException("Phiếu đăng ký đã hoàn tất điểm danh hoặc đã bị hủy trước đó, không thể hủy lại.")`.

---

#### 🔹 US-NEW-02: Rút tên khỏi Câu lạc bộ (Leave Club)
* **User Story:**
  > *"Là một **Thành viên của CLB**, tôi muốn **xin rút tên khỏi danh sách câu lạc bộ**, để **cập nhật lại sĩ số chính thức và giải phóng thông tin hoạt động phong trào**."*

* **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **AC-02.1 (Rút tên thành công):**
    - **Given:** Sinh viên đang là thành viên chính thức có mặt trong danh sách `club.Members`.
    - **When:** Sinh viên thực hiện thao tác rời CLB qua lệnh `club.RemoveMember(memberId)`.
    - **Then:** Sinh viên bị loại khỏi danh sách `Members`, tổng sĩ số `club.MemberCount()` giảm đi 1, nhưng toàn bộ lịch sử các sự kiện sinh viên từng tham gia trong quá khứ vẫn được bảo toàn nguyên vẹn.
  - **AC-02.2 (Chặn thao tác khi không phải là thành viên):**
    - **Given:** Sinh viên chưa từng gia nhập hoặc đã rời CLB này từ trước.
    - **When:** Sinh viên gọi phương thức rút tên khỏi CLB.
    - **Then:** Hệ thống báo lỗi `InvalidOperationException("Sinh viên này hiện không phải là thành viên của CLB.")`.

---

### 🧑‍💼 NHÓM 2: TÁC NHÂN BAN CHỦ NHIỆM CLB (CLUB ADMIN)

#### 🔹 US-NEW-03: Hủy bỏ sự kiện đã lên lịch (Cancel Scheduled Event)
* **User Story:**
  > *"Là **Ban chủ nhiệm CLB**, tôi muốn **hủy bỏ một sự kiện đã lên lịch khi có sự cố bất khả kháng**, để **hệ thống tự động thông báo và hủy toàn bộ các phiếu đăng ký liên quan**."*

* **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **AC-03.1 (Hủy sự kiện hợp lệ trước giờ G):**
    - **Given:** Sự kiện thuộc quyền quản lý của CLB, chưa tới thời gian tổ chức và có danh sách các phiếu đăng ký ở trạng thái `Pending` / `Confirmed`.
    - **When:** Ban chủ nhiệm kích hoạt thao tác hủy sự kiện.
    - **Then:** Sự kiện chuyển trạng thái sang đã hủy, toàn bộ phiếu đăng ký gắn với `EventId` đó tự động được chuyển trạng thái sang `RegistrationStatus.Cancelled`.
  - **AC-03.2 (Chặn hủy khi sự kiện đã/đang diễn ra):**
    - **Given:** Thời gian tổ chức sự kiện (`Event.Date`) đã qua trong quá khứ hoặc đã có ít nhất một sinh viên `CheckIn`.
    - **When:** Ban chủ nhiệm cố gắng hủy sự kiện.
    - **Then:** Hệ thống từ chối và ném ra ngoại lệ `InvalidOperationException("Không thể hủy sự kiện đã diễn ra hoặc đã phát sinh lượt check-in thực tế.")`.

---

#### 🔹 US-NEW-04: Duyệt hàng loạt phiếu đăng ký theo sức chứa (Batch Confirm Registrations)
* **User Story:**
  > *"Là **Ban chủ nhiệm CLB**, tôi muốn **xác nhận duyệt đồng loạt các phiếu đăng ký đang chờ (Pending) trong phạm vi sức chứa còn lại**, để **chốt danh sách người tham gia chính thức mà không bị vượt chỉ tiêu**."*

* **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **AC-04.1 (Duyệt thành công khi còn đủ chỗ):**
    - **Given:** Sự kiện có sức chứa `Capacity = 100`, hiện tại đã có 90 phiếu `Confirmed` (còn 10 chỗ trống) và có 8 phiếu đang ở trạng thái `Pending`.
    - **When:** Ban chủ nhiệm chọn xác nhận duyệt cả 8 phiếu `Pending`.
    - **Then:** Cả 8 phiếu đều được chuyển thành công sang `Confirmed`, số người được xác nhận nâng lên 98/100 và sẵn sàng cho phép check-in.
  - **AC-04.2 (Chặn duyệt vượt quá sức chứa Capacity):**
    - **Given:** Sự kiện chỉ còn 2 chỗ trống nhưng ban chủ nhiệm cố tình chọn duyệt 5 phiếu `Pending`.
    - **When:** Hệ thống thực thi quy trình duyệt theo thứ tự thời gian đăng ký (`RegisteredAt`).
    - **Then:** Hệ thống chỉ duyệt đúng 2 phiếu đầu tiên đạt ngưỡng sức chứa, 3 phiếu còn lại giữ nguyên trạng thái `Pending` và ném ra cảnh báo `InvalidOperationException("Đã đạt giới hạn sức chứa tối đa của sự kiện, không thể duyệt thêm.")`.

---

### 🛠 NHÓM 3: TÁC NHÂN QUẢN TRỊ VIÊN HỆ THỐNG (SYSTEM ADMIN)

#### 🔹 US-NEW-05: Phê duyệt thành lập Câu lạc bộ mới (Create & Authorize Club)
* **User Story:**
  > *"Là **Quản trị viên hệ thống (Admin)**, tôi muốn **khởi tạo và phê duyệt một Câu lạc bộ sinh viên mới với tên gọi và mô tả hợp lệ**, để **cho phép CLB bắt đầu chiêu mộ thành viên và tổ chức phong trào**."*

* **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **AC-05.1 (Khởi tạo CLB hợp lệ thành công):**
    - **Given:** Admin cung cấp tên CLB hợp lệ (không rỗng) và nội dung giới thiệu tôn chỉ hoạt động.
    - **When:** Admin gọi phương thức tĩnh `Club.Create(name, description)`.
    - **Then:** Đối tượng `Club` được tạo thành công với mã `Id` (Guid) duy nhất, ngày tạo `CreatedAt` ghi nhận thời gian hiện tại và danh sách `Members` ban đầu có số lượng bằng 0.
  - **AC-05.2 (Từ chối khởi tạo khi dữ liệu rỗng):**
    - **Given:** Tên CLB bị bỏ trống (`null`), chuỗi rỗng (`""`), hoặc chỉ chứa khoảng trắng (`"   "`).
    - **When:** Admin gửi yêu cầu khởi tạo CLB.
    - **Then:** Phương thức `Club.Create()` lập tức ném ra ngoại lệ `ArgumentException("Tên câu lạc bộ không được để trống.", nameof(name))` và chặn đứng việc tạo đối tượng rác.

---

#### 🔹 US-NEW-06: Khôi phục hoạt động cho thành viên (Reactivate Member)
* **User Story:**
  > *"Là **Quản trị viên hệ thống (Admin)**, tôi muốn **khôi phục trạng thái hoạt động (Active) cho sinh viên từng bị tạm ngưng**, để **sinh viên tiếp tục quyền lợi tham gia CLB và đăng ký sự kiện sau khi đã khắc phục vi phạm**."*

* **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **AC-06.1 (Khôi phục thành công từ Inactive sang Active):**
    - **Given:** Sinh viên đang bị khóa tài khoản hoặc tạm ngưng (`Status == MemberStatus.Inactive`).
    - **When:** Admin thực hiện lệnh khôi phục `member.Reactivate()`.
    - **Then:** Thuộc tính `Status` chuyển thành `MemberStatus.Active`, hàm kiểm tra `member.IsActive()` trả về kết quả `true`.
  - **AC-06.2 (Bảo toàn khi tài khoản vốn đang Active):**
    - **Given:** Sinh viên đang ở trạng thái bình thường (`Status == MemberStatus.Active`).
    - **When:** Admin vô tình thực hiện thao tác khôi phục lại.
    - **Then:** Hệ thống không gây lỗi crash, giữ nguyên trạng thái `Active` và thông báo cho Admin biết tài khoản vốn đang hoạt động bình thường.
