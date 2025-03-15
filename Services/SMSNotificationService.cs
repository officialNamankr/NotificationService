using NotificationService.Models;
using NotificationService.Services.IServices;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace NotificationService.Services
{
    public class SMSNotificationService : INotificationService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhone;

        public SMSNotificationService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"] ?? throw new Exception("Twilio AccountSid is missing");
            _authToken = configuration["Twilio:AuthToken"] ?? throw new Exception("Twilio AuthToken is missing");
            _fromPhone = configuration["Twilio:FromPhone"] ?? throw new Exception("Twilio FromPhone is missing");
            TwilioClient.Init(_accountSid, _authToken);
        }
        public async Task<bool> SendNotificationAsync(NotificationMessage message)
        {
            var smsMessage = await MessageResource.CreateAsync(
                body: message.Body,
                from: new Twilio.Types.PhoneNumber(_fromPhone),
                to: new Twilio.Types.PhoneNumber(message.Recipient)
            );
            return smsMessage.Status != MessageResource.StatusEnum.Failed;
        }
    }
}
