namespace EduHub.Domain;

/// <summary>
/// Đại diện cho bản ghi điểm danh sinh viên tại sự kiện
/// </summary>
public class CheckIn
{
    public Guid Id { get; private set; }
    public Guid RegistrationId { get; private set; } // Khóa ngoại FK tham chiếu Registration
    public DateTime CheckedInAt { get; private set; }
    public CheckInMethod Method { get; private set; }

    private CheckIn(Guid registrationId, CheckInMethod method)
    {
        Id = Guid.NewGuid();
        RegistrationId = registrationId;
        CheckedInAt = DateTime.UtcNow;
        Method = method;
    }

    /// <summary>
    /// Factory Method ghi nhận Check-in: BẮT BUỘC phiếu đăng ký phải ở trạng thái Confirmed
    /// </summary>
    public static CheckIn Create(Registration registration, CheckInMethod method = CheckInMethod.QRCode)
    {
        ArgumentNullException.ThrowIfNull(registration);

        if (registration.Status != RegistrationStatus.Confirmed)
            throw new InvalidOperationException($"Chỉ phiếu đăng ký đã xác nhận (Confirmed) mới được phép check-in. Trạng thái hiện tại: {registration.Status}.");

        return new CheckIn(registration.Id, method);
    }

    public string Summary() => 
        $"CheckIn: {Id:N} | Phiếu: {RegistrationId:N} | Lúc: {CheckedInAt:HH:mm:ss dd/MM/yyyy} | Phương thức: {Method}";

    public override string ToString() => Summary();
}
