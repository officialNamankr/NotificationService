using FluentValidation;
using NotificationService.Models;
using NotificationService.Cross_Cuttings;
namespace NotificationService.Validator
{
    public class NotificationMessageValidator : AbstractValidator<NotificationMessage>
    {
        public NotificationMessageValidator() 
        { 
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required").Must(category => Cross_Cuttings.CrossCutting_Constants.NotificationTypes.Contains(category)).WithMessage("Invalid Notifcation Type");
            RuleFor(x => x.Recipient).NotEmpty().WithMessage("Recipient is required").EmailAddress().WithMessage("Invalid email format").When(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase)); ;
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Subject is required").When(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase));
            RuleFor(x => x.Body).NotEmpty().WithMessage("Body is required");
        }
    }
}
