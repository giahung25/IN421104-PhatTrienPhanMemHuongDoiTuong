# 🎓 EDUHUB — BÀI TẬP PHÂN TÍCH YÊU CẦU & USER STORIES
## HỌC PHẦN: PHÁT TRIỂN PHẦN MỀM HƯỚNG ĐỐI TƯỢNG (IN4211)
**Trường Đại học Đồng Tháp — Khoa Công nghệ và Kỹ thuật**  
* **Giảng viên hướng dẫn:** ThS. Nguyễn Tuấn Kiệt
* **Sinh viên thực hiện:** **Phan Trần Gia Hưng**
* **Mã số sinh viên (MSSV):** **`0024417689`**
* **Lớp:** DHCNTT24B (hoặc DHCNTT24A)
* **Chủ đề nộp:** Bài tập Chương 1 (Slide trang 34) — Xây dựng bộ User Stories cho dự án EduHub

---

## 📌 1. TỔNG QUAN BÀI LÀM
Thực hiện yêu cầu bài tập tại **Slide trang 34**:
> *"Tự viết ít nhất 5 story, đủ 3 vai trò. Đừng chép story mẫu!"*

Bộ User Stories dưới đây được xây dựng độc lập hoàn toàn 100%, bao phủ đầy đủ 3 tác nhân trong bài toán EduHub (Sinh viên, Ban chủ nhiệm CLB, Quản trị viên hệ thống) kèm tiêu chí nghiệm thu chi tiết (Acceptance Criteria).

---

## 📊 2. BẢNG TỔNG QUAN 6 USER STORIES CHO 3 VAI TRÒ (ACTORS)

| STT | Mã Story | Tác nhân (Actor) | Tính năng nghiệp vụ | Tóm tắt mục đích & Giá trị nghiệp vụ |
| :---: | :---: | :--- | :--- | :--- |
| **1** | **US-NEW-01** | 👤 Sinh viên (Member) | Hủy phiếu đăng ký sự kiện | Sinh viên bận đột xuất muốn hủy phiếu trước giờ G để nhường suất tham dự cho người khác. |
| **2** | **US-NEW-02** | 👤 Sinh viên (Member) | Rút tên khỏi Câu lạc bộ | Thành viên chủ động rời CLB khi không còn thời gian sinh hoạt, cập nhật lại sĩ số chính thức. |
| **3** | **US-NEW-03** | 🧑‍💼 Ban CLB (Club Admin) | Hủy bỏ sự kiện đã lên lịch | Ban tổ chức hủy sự kiện do sự cố bất khả kháng, tự động chuyển toàn bộ phiếu sang Cancelled. |
| **4** | **US-NEW-04** | 🧑‍💼 Ban CLB (Club Admin) | Duyệt phiếu theo sức chứa | Xác nhận hàng loạt phiếu Pending trong giới hạn Capacity còn lại của sự kiện, chặn vượt chỉ tiêu. |
| **5** | **US-NEW-05** | 🛠 Quản trị viên (Admin) | Phê duyệt thành lập CLB mới | Khởi tạo tổ chức CLB mới với kiểm tra toàn vẹn định danh (Name không rỗng, Id GUID duy nhất). |
| **6** | **US-NEW-06** | 🛠 Quản trị viên (Admin) | Khôi phục thành viên (Reactivate) | Kích hoạt lại tài khoản thành viên từ Inactive sang Active sau khi hoàn thành thủ tục kỷ luật. |

---

## 📝 3. ĐẶC TẢ CHI TIẾT USER STORIES (BỐ CỤC CHUẨN SLIDE 32 & 33)

### **3.4 User Story — Sinh viên (Bố cục Slide 32)**

#### **US-NEW-01 · Sinh viên: Hủy phiếu đăng ký sự kiện**
* ① Phiếu `Pending` / `Confirmed` ➜ hủy phiếu ➜ chuyển `Cancelled` & hoàn lại chỗ trống (+1)
* ② Phiếu đã `CheckIn` / `Cancelled` ➜ hủy phiếu ➜ bị từ chối (báo lỗi không thể hủy)

#### **US-NEW-02 · Sinh viên: Rút tên khỏi Câu lạc bộ**
* ① Đang là thành viên ➜ rút tên ➜ xóa khỏi `club.Members` & sĩ số `MemberCount()` giảm 1
* ② Chưa là thành viên ➜ rút tên ➜ bị từ chối (báo lỗi không thuộc CLB)

---

### **3.5 User Story — Ban CLB & Admin (Bố cục Slide 33)**

#### **US-NEW-03 · Ban CLB: Hủy bỏ sự kiện đã lên lịch**
* ① Sự kiện chưa diễn ra ➜ hủy sự kiện ➜ sự kiện hủy & mọi phiếu chuyển `Cancelled`
* ② Sự kiện đã qua ngày / đã check-in ➜ hủy sự kiện ➜ bị từ chối (không thể hủy)

#### **US-NEW-04 · Ban CLB: Duyệt hàng loạt phiếu đăng ký theo sức chứa**
* ① Còn chỗ (`HasCapacity == true`) ➜ duyệt ➜ các phiếu chuyển `Confirmed`
* ② Đã đủ `Capacity` ➜ duyệt tiếp ➜ báo hết chỗ (giữ nguyên trạng thái `Pending`)

#### **US-NEW-05 · Admin: Phê duyệt thành lập CLB mới**
* ① Tên hợp lệ, có mô tả ➜ tạo CLB ➜ tạo `Club` mới với `Id` (Guid) & `Members` rỗng
* ② Tên trống / rỗng ➜ tạo CLB ➜ báo lỗi (`ArgumentException`: Tên không được trống)

#### **US-NEW-06 · Admin: Khôi phục hoạt động cho thành viên**
* ① Thành viên đang `Inactive` ➜ khôi phục ➜ chuyển `Active` & `IsActive() = true`
* ② Thành viên đang `Active` ➜ khôi phục ➜ giữ nguyên `Active` & thông báo bình thường

---

## 📁 4. TÀI LIỆU BÀI NỘP ĐÍNH KÈM
* **File Word bảng biểu:** `BO_USER_STORIES_MOI_CHO_EDUHUB.docx`
* **File Markdown chi tiết:** `USER_STORIES_TRANG_34_EDUHUB.md`
* **Quy chuẩn lập trình áp dụng:** `CODING_RULES_IN4211.md`
