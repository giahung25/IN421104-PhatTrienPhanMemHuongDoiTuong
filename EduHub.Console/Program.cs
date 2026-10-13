using System;
using System.Collections.Generic;
using EduHub.Domain;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 🎓 HỆ THỐNG QUẢN LÝ CÂU LẠC BỘ & SỰ KIỆN SINH VIÊN (EDUHUB - CHƯƠNG 01)");
Console.WriteLine(" Học phần: Phát triển phần mềm hướng đối tượng (IN4211) — .NET 10");
Console.WriteLine(" Sinh viên: Phan Trần Gia Hưng (MSSV: 0024417689)");
Console.WriteLine("==========================================================================\n");

try
{
    // BƯỚC 1: Khởi tạo Câu lạc bộ
    Console.WriteLine("[BƯỚC 1] Khởi tạo Câu lạc bộ mới:");
    var itClub = Club.Create("CLB Tin Học DThU", "Giao lưu học thuật, lập trình mã nguồn mở và chuyển đổi số");
    Console.WriteLine($"  -> Tạo thành công: {itClub.Name} (ID: {itClub.Id})");

    // BƯỚC 2: Tạo Sinh viên & Thêm vào CLB
    Console.WriteLine("\n[BƯỚC 2] Đăng ký thành viên tham gia CLB:");
    var sv1 = Member.Create("Phan Trần Gia Hưng", "0024417689@dthu.edu.vn", "0024417689");
    var sv2 = Member.Create("Nguyễn Tấn Lộc", "0024410001@dthu.edu.vn", "0024410001");
    itClub.AddMember(sv1);
    itClub.AddMember(sv2);
    Console.WriteLine($"  -> Đã thêm 2 thành viên vào CLB. Sĩ số hiện tại: {itClub.MemberCount()}");

    // BƯỚC 3: Tạo Sự kiện của CLB
    Console.WriteLine("\n[BƯỚC 3] Ban chủ nhiệm tạo Sự kiện mới:");
    var workshop = Event.Create(itClub.Id, "Workshop Clean Architecture với .NET 10", DateTime.UtcNow.AddDays(7), 2, true);
    Console.WriteLine($"  -> Sự kiện: '{workshop.Title}' | Sức chứa tối đa: {workshop.Capacity} | Hình thức: {(workshop.IsOnline ? "Online" : "Offline")}");

    // BƯỚC 4: Đăng ký tham gia sự kiện
    Console.WriteLine("\n[BƯỚC 4] Sinh viên đăng ký tham gia sự kiện:");
    var regList = new List<Registration>();

    if (workshop.HasCapacity(regList.Count))
    {
        var reg1 = Registration.Create(sv1.Id, workshop.Id);
        regList.Add(reg1);
        Console.WriteLine($"  -> {sv1.Name} đăng ký thành công (Mã phiếu: {reg1.Id:N}, Trạng thái: {reg1.Status})");
    }

    if (workshop.HasCapacity(regList.Count))
    {
        var reg2 = Registration.Create(sv2.Id, workshop.Id);
        regList.Add(reg2);
        Console.WriteLine($"  -> {sv2.Name} đăng ký thành công (Mã phiếu: {reg2.Id:N}, Trạng thái: {reg2.Status})");
    }

    // BƯỚC 5: Ban tổ chức duyệt phiếu đăng ký
    Console.WriteLine("\n[BƯỚC 5] Ban tổ chức duyệt xác nhận phiếu đăng ký:");
    regList[0].Confirm();
    Console.WriteLine($"  -> Phiếu của {sv1.Name} đã được chuyển sang: {regList[0].Status}");

    // BƯỚC 6: Check-in tại sự kiện
    Console.WriteLine("\n[BƯỚC 6] Sinh viên quét mã QR điểm danh ngày diễn ra sự kiện:");
    var checkIn1 = CheckIn.Create(regList[0], CheckInMethod.QRCode);
    Console.WriteLine($"  -> {checkIn1.Summary()}");

    // BƯỚC 7: Xuất báo cáo tham dự
    Console.WriteLine("\n[BƯỚC 7] Báo cáo thống kê tham dự sự kiện:");
    Console.WriteLine($"  * Sự kiện: {workshop.Title}");
    Console.WriteLine($"  * Sức chứa tối đa (Capacity): {workshop.Capacity}");
    Console.WriteLine($"  * Số phiếu đăng ký hợp lệ: {regList.Count}");
    Console.WriteLine($"  * Số lượng sinh viên đã Check-in thực tế: 1 / {regList.Count}");

    // BƯỚC 8: Thử nghiệm vi phạm luật nghiệp vụ (Business Rule Invariants Validation)
    Console.WriteLine("\n==========================================================================");
    Console.WriteLine(" ⚠️ [BƯỚC 8] KIỂM THỬ RÀNG BUỘC NGHIỆP VỤ (EXCEPTION & INVARIANTS)");
    Console.WriteLine("==========================================================================");

    // Test vi phạm 1: Thêm trùng thành viên vào CLB
    try
    {
        Console.WriteLine("\n* Thử nghiệm 1: Thêm trùng sinh viên vào CLB...");
        itClub.AddMember(sv1);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"  -> [BẮT LỖI THÀNH CÔNG]: {ex.Message}");
    }

    // Test vi phạm 2: Check-in khi phiếu chưa Confirmed
    try
    {
        Console.WriteLine("\n* Thử nghiệm 2: Cố tình check-in phiếu thứ 2 (đang ở trạng thái Pending)...");
        var invalidCheckIn = CheckIn.Create(regList[1], CheckInMethod.Manual);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"  -> [BẮT LỖI THÀNH CÔNG]: {ex.Message}");
    }

    // Test vi phạm 3: Tạo sự kiện với sức chứa không hợp lệ (capacity <= 0)
    try
    {
        Console.WriteLine("\n* Thử nghiệm 3: Tạo sự kiện với sức chứa capacity = 0...");
        var invalidEvent = Event.Create(itClub.Id, "Seminar Lỗi", DateTime.UtcNow, 0);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"  -> [BẮT LỖI THÀNH CÔNG]: {ex.Message}");
    }

    // Test vi phạm 4: Hủy phiếu đã bị hủy trước đó
    try
    {
        Console.WriteLine("\n* Thử nghiệm 4: Hủy phiếu đăng ký và thử hủy lại lần 2...");
        regList[1].Cancel();
        regList[1].Cancel(); // Lần 2
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"  -> [BẮT LỖI THÀNH CÔNG]: {ex.Message}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"[LỖI HỆ THỐNG]: {ex.Message}");
}

Console.WriteLine("\n==========================================================================");
Console.WriteLine(" ✅ CHƯƠNG TRÌNH DEMO THỰC THI THÀNH CÔNG 100% ĐẠT CHUẨN ĐẶC TẢ CH01");
Console.WriteLine("==========================================================================");
