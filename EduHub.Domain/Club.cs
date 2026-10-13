namespace EduHub.Domain;

/// <summary>
/// Đại diện cho Câu lạc bộ sinh viên
/// </summary>
public class Club
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    // Đóng gói tập hợp: private field, public IReadOnlyList ra ngoài
    private readonly List<Member> _members = new();
    public IReadOnlyList<Member> Members => _members.AsReadOnly();

    private Club(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Factory Method khởi tạo Câu lạc bộ
    /// </summary>
    public static Club Create(string name, string description = "")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên câu lạc bộ không được để trống.", nameof(name));

        return new Club(name.Trim(), description?.Trim() ?? string.Empty);
    }

    /// <summary>
    /// Thêm thành viên vào CLB với kiểm tra trùng lặp
    /// </summary>
    public void AddMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);

        if (_members.Any(m => m.Id == member.Id || m.StudentCode == member.StudentCode))
            throw new InvalidOperationException($"Sinh viên '{member.Name}' ({member.StudentCode}) đã là thành viên của CLB.");

        _members.Add(member);
    }

    /// <summary>
    /// Cho phép sinh viên rút tên khỏi CLB
    /// </summary>
    public void RemoveMember(Guid memberId)
    {
        var existing = _members.FirstOrDefault(m => m.Id == memberId);
        if (existing is null)
            throw new InvalidOperationException("Sinh viên này hiện không phải là thành viên của CLB.");

        _members.Remove(existing);
    }

    public int MemberCount() => _members.Count;

    public override string ToString() => $"{Name} (Sĩ số: {MemberCount()})";
}
