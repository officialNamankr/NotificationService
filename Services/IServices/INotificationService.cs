using NotificationService.Models;

namespace NotificationService.Services.IServices
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(NotificationMessage message);
    }
}
