using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using NotificationService.Models.Dto;
using NotificationService.Services.IServices;

namespace NotificationService.Services
{

   
    public class LogService : ILogService
    {
        private readonly string _serviceBusConnectionString;
        private readonly String _queueName;

        public LogService(IConfiguration configuration)
        {
            _serviceBusConnectionString = configuration["ServiceBus:ConnectionString"] ?? throw new ArgumentNullException("ServiceBus Connection String is missing");
            _queueName = configuration["ServiceBus:QueueName"] ?? throw new ArgumentNullException("ServiceBus Queue Name is missing");
        }

        public async Task SendLogAsync(string filename, string type, string logMessage)
        {
            await using var client = new ServiceBusClient(_serviceBusConnectionString);
            ServiceBusSender sender = client.CreateSender(_queueName);

            var logData = new LogMessageModel
            {
                Type = type,
                FileName = filename,
                Message = logMessage
            };

            string messageBody = JsonSerializer.Serialize(logData);

            ServiceBusMessage message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));
            await sender.SendMessageAsync(message);

        }
    }
}
