using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotificationService.Services;
using NotificationService.Services.IServices;

namespace NotificationService.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, ILogService logService)
        {
            var requestBody = await ReadRequestBody(context);
            var requestLog = $"[{DateTime.UtcNow}] Request: {context.Request.Method} {context.Request.Path}\nBody: {requestBody}\n";

            var originalResponseBody = context.Response.Body;
            using var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;

            await _next(context);

            var responseBody = await ReadResponseBody(context);
            var responseLog = $"[{DateTime.UtcNow}] Response: {context.Response.StatusCode}\nBody: {responseBody}\n";

            await newResponseBody.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;

            var recipient = GetRecipient(requestBody);
            var messageType = GetMessageType(requestBody);

            await logService.SendLogAsync(recipient, messageType, requestLog + responseLog);
        }


        private async Task<string> ReadRequestBody(HttpContext context)
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body,Encoding.UTF8, leaveOpen:true).ReadToEndAsync();
            context.Request.Body.Position = 0;
            return body;
        }

        private async Task<string> ReadResponseBody(HttpContext context)
        {
           
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            return body;
            
        }

        private string GetRecipient(string requestbody)
        {
            if (string.IsNullOrWhiteSpace(requestbody))
            {
                Console.WriteLine("Request body is empty or null.");
                return "Unknown";
            }
            try
            {
                var json = JObject.Parse(requestbody);
                return json["recipient"]?.ToString() ?? "Unknown";
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine("Request body is not a valid JSON."+e.Message);
                return "Unknown";
            }
        }

        private string GetMessageType(string requestbody)
        {
            if (string.IsNullOrWhiteSpace(requestbody))
            {
                Console.WriteLine("Request body is empty or null.");
                return "Unknown";
            }
            try
            {
                var json = JObject.Parse(requestbody);
                return json["type"]?.ToString() ?? "Unknown";
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine("Request body is not a valid JSON." + e.Message);
                return "Unknown";
            }
        }

    }
}
