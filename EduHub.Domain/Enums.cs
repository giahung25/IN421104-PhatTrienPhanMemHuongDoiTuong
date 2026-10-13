namespace EduHub.Domain;

/// <summary>
/// Trạng thái hoạt động của thành viên trong hệ thống
/// </summary>
public enum MemberStatus
{
    Inactive = 0,
    Active = 1
}

/// <summary>
/// Vòng đời trạng thái của phiếu đăng ký sự kiện
/// </summary>
public enum RegistrationStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2
}

/// <summary>
/// Phương thức ghi nhận điểm danh tại sự kiện
/// </summary>
public enum CheckInMethod
{
    Manual = 0,
    QRCode = 1
}
