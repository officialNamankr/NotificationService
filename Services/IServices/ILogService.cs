namespace NotificationService.Services.IServices
{
    public interface ILogService
    {
        Task SendLogAsync(string fileName,string type, string logMessage);
    }
}
