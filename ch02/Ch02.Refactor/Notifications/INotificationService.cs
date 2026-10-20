namespace EduHub.Ch02.Notifications;

/// <summary>
/// Gửi thông báo. Khi muốn bổ sung kênh mới (SMS, Slack), chỉ cần thêm class mới mà không cần sửa code cũ (OCP).
/// </summary>
public interface INotificationService
{
    void Send(string to, string message);
}
