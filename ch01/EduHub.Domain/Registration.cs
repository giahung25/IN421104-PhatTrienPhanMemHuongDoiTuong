namespace EduHub.Domain;

/// <summary>
/// Đại diện cho Phiếu đăng ký tham gia sự kiện của sinh viên
/// </summary>
public class Registration
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; } // Khóa ngoại FK tham chiếu Member
    public Guid EventId { get; private set; }  // Khóa ngoại FK tham chiếu Event
    public DateTime RegisteredAt { get; private set; }
    public RegistrationStatus Status { get; private set; }

    private Registration(Guid memberId, Guid eventId)
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        EventId = eventId;
        RegisteredAt = DateTime.UtcNow;
        Status = RegistrationStatus.Pending;
    }

    /// <summary>
    /// Factory Method khởi tạo Phiếu đăng ký với trạng thái ban đầu là Pending
    /// </summary>
    public static Registration Create(Guid memberId, Guid eventId)
    {
        if (memberId == Guid.Empty)
            throw new ArgumentException("Mã thành viên (MemberId) không hợp lệ.", nameof(memberId));

        if (eventId == Guid.Empty)
            throw new ArgumentException("Mã sự kiện (EventId) không hợp lệ.", nameof(eventId));

        return new Registration(memberId, eventId);
    }

    /// <summary>
    /// Ban tổ chức xác nhận phiếu đăng ký
    /// </summary>
    public void Confirm()
    {
        if (Status != RegistrationStatus.Pending)
            throw new InvalidOperationException($"Chỉ có thể xác nhận phiếu đang ở trạng thái Pending. Trạng thái hiện tại: {Status}.");

        Status = RegistrationStatus.Confirmed;
    }

    /// <summary>
    /// Hủy phiếu đăng ký (cho phép khi Pending hoặc Confirmed)
    /// </summary>
    public void Cancel()
    {
        if (Status == RegistrationStatus.Cancelled)
            throw new InvalidOperationException("Phiếu đăng ký này đã bị hủy trước đó.");

        Status = RegistrationStatus.Cancelled;
    }

    public override string ToString() => $"[Registration] ID: {Id:N} | Status: {Status} | RegAt: {RegisteredAt:HH:mm dd/MM}";
}
