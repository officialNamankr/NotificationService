using NotificationService.Services.IServices;

namespace NotificationService.Services
{
    public class NotificationServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public NotificationServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public INotificationService GetNotificationService(string type)
        {
            return type.ToLower() switch
            {
                "email" => _serviceProvider.GetService<EmailNotificationService>(),
                "sms" => _serviceProvider.GetService<SMSNotificationService>(),
                _ => throw new NotSupportedException("Notification type not supported")
            };
        }
    }
}
