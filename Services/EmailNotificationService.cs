using NotificationService.Models;
using NotificationService.Services.IServices;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationService.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly string _apiKey;

        public EmailNotificationService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"] ?? throw new ArgumentNullException("SendGrid API key is missing");
        }
        public async Task<bool> SendNotificationAsync(NotificationMessage message)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("no-reply@example.com", "Notification Service");
            var to = new EmailAddress(message.Recipient);
            var emailMessage = MailHelper.CreateSingleEmail(from, to, message.Subject, message.Body, message.Body);
            var response = await client.SendEmailAsync(emailMessage);

            return response.IsSuccessStatusCode;
        }

    }
}
