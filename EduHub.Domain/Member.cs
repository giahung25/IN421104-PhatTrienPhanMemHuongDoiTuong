namespace EduHub.Domain;

/// <summary>
/// Đại diện cho Sinh viên / Thành viên trong hệ thống EduHub
/// </summary>
public class Member
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string StudentCode { get; private set; } = string.Empty;
    public DateTime JoinedAt { get; private set; }
    public MemberStatus Status { get; private set; }

    // Constructor private để ép buộc khởi tạo qua Factory Method Create()
    private Member(string name, string email, string studentCode)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        StudentCode = studentCode;
        JoinedAt = DateTime.UtcNow;
        Status = MemberStatus.Active;
    }

    /// <summary>
    /// Factory Method khởi tạo Member có kiểm tra toàn vẹn dữ liệu (Invariants Validation)
    /// </summary>
    public static Member Create(string name, string email, string studentCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên thành viên không được để trống.", nameof(name));
        
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email không đúng định dạng.", nameof(email));
            
        if (string.IsNullOrWhiteSpace(studentCode))
            throw new ArgumentException("Mã số sinh viên không được để trống.", nameof(studentCode));

        return new Member(
            name.Trim(),
            email.Trim().ToLowerInvariant(),
            studentCode.Trim().ToUpperInvariant()
        );
    }

    public bool IsActive() => Status == MemberStatus.Active;

    public void Deactivate()
    {
        Status = MemberStatus.Inactive;
    }

    public void Reactivate()
    {
        Status = MemberStatus.Active;
    }

    public override string ToString() => $"{Name} ({StudentCode})";
}
