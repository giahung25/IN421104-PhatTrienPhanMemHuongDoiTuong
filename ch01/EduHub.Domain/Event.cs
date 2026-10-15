namespace EduHub.Domain;

/// <summary>
/// Đại diện cho Sự kiện do CLB tổ chức
/// </summary>
public class Event
{
    public Guid Id { get; private set; }
    public Guid ClubId { get; private set; } // Khóa ngoại FK tham chiếu về Club
    public string Title { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }
    public int Capacity { get; private set; }
    public bool IsOnline { get; private set; }
    public bool IsCancelled { get; private set; }

    private Event(Guid clubId, string title, DateTime date, int capacity, bool isOnline)
    {
        Id = Guid.NewGuid();
        ClubId = clubId;
        Title = title;
        Date = date;
        Capacity = capacity;
        IsOnline = isOnline;
        IsCancelled = false;
    }

    /// <summary>
    /// Factory Method khởi tạo Sự kiện với ràng buộc sức chứa và tiêu đề
    /// </summary>
    public static Event Create(Guid clubId, string title, DateTime date, int capacity, bool isOnline = false)
    {
        if (clubId == Guid.Empty)
            throw new ArgumentException("Mã câu lạc bộ (ClubId) không hợp lệ.", nameof(clubId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề sự kiện không được để trống.", nameof(title));

        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Sức chứa sự kiện phải lớn hơn 0.");

        return new Event(clubId, title.Trim(), date, capacity, isOnline);
    }

    /// <summary>
    /// Kiểm tra xem sự kiện còn chỗ trống hay không
    /// </summary>
    public bool HasCapacity(int registeredCount) => !IsCancelled && registeredCount < Capacity;

    /// <summary>
    /// Hủy sự kiện khi chưa diễn ra
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
            throw new InvalidOperationException("Sự kiện này đã bị hủy trước đó.");

        if (DateTime.UtcNow > Date)
            throw new InvalidOperationException("Không thể hủy sự kiện đã diễn ra trong quá khứ.");

        IsCancelled = true;
    }

    public override string ToString() => $"[Event] {Title} (Capacity: {Capacity}, Online: {IsOnline})";
}
