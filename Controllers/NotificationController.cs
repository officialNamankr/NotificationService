using Microsoft.AspNetCore.Mvc;
using NotificationService.Models.Dto;
using NotificationService.Services;

namespace NotificationService.Controllers
{

    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationServiceFactory _notificationServiceFactory;
        private ResponseDto _responseDto;

        public NotificationController(NotificationServiceFactory factory)
        {
            _notificationServiceFactory = factory;
            _responseDto = new ResponseDto();
        }
        
        [HttpPost]
        public async Task<ResponseDto> SendNotification([FromBody] Models.NotificationMessage message)
        {
            try
            {
                var service = _notificationServiceFactory.GetNotificationService(message.Type);
                var success = await service.SendNotificationAsync(message);
                if (success)
                {
                    _responseDto.DisplayMessage = "Notification sent successfully";
                    return _responseDto;
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.DisplayMessage = "Failed to send notification";
                    _responseDto.ErrorMessages = new List<string> { "Failed to send notification" };
                    return _responseDto;

                }
            }
            catch (NotSupportedException ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.DisplayMessage = ex.Message;
                _responseDto.ErrorMessages = new List<string> { ex.Message };
                return _responseDto;

            }
        }
    }
}
