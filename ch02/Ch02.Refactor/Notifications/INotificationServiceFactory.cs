namespace EduHub.Ch02.Notifications;

/// <summary>
/// FACTORY METHOD: nơi định nghĩa hợp đồng tạo từng loại INotificationService.
/// </summary>
public interface INotificationServiceFactory
{
    INotificationService Create(NotificationChannel channel);
}
