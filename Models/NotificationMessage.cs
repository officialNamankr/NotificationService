namespace NotificationService.Models
{
    public class NotificationMessage
    {
        public string Type { get; set; } = "";
        public string Recipient { get; set; } = "";
        public string Subject { get; set; } = "";

        public string Body { get; set; } = "";
    }
}
