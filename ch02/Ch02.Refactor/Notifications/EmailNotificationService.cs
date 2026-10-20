namespace EduHub.Ch02.Notifications;

/// <summary>
/// Cài đặt thông báo qua Email — thay thế cho chuỗi if/else phân biệt Gmail/Yahoo trong mã nguồn cũ.
/// </summary>
public class EmailNotificationService : INotificationService
{
    public void Send(string to, string message) =>
        Console.WriteLine($"  [EMAIL] to {to}: {message}");
}
