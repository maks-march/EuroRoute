using Application.Common.Extensions;
using Domain.Enums;
using FluentValidation;

namespace Application.CQRS.OrderCQ.Commands.Update.Validators;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .When(x => x.StartDate != null)
            .WithMessage("Start date is required.")
            .GreaterThan(DateTime.Now.ToDateOnly())
            .When(x => x.StartDate != null)
            .WithMessage("Start date must be in the future.");
        
        RuleFor(x => x.Status)
            .MaximumLength(20)
            .WithMessage("Status must be 20 characters or fewer.")
            .Must(x => Enum.IsDefined(typeof(OrderStatus), x))
            .When(x => x.Status != null)
            .WithMessage("Status must be one of the following.");

        RuleFor(x => x.SpecNumber)
            .InclusiveBetween(100, 100000)
            .When(x => x.SpecNumber != null)
            .WithMessage("SpecNumber must be greater than 100.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Author ID is required.");
        
        RuleFor(x => x.About)
            .MaximumLength(200)
            .WithMessage("About must be 200 characters or fewer.");
        
        RuleFor(x => x.Payloads)
            .NotEmpty()
            .When(x => x.Payloads != null)
            .WithMessage("At least one payload is required.");
        
        RuleFor(x => x.RoutePoints)
            .NotEmpty()
            .When(x => x.RoutePoints != null)
            .WithMessage("At least two route points are required (start and end).")
            .Must(points => points == null || points.Count >= 2)
            .WithMessage("At least two route points are required (start and end).");

        RuleFor(x => x.Payment)
            .SetValidator(new PaymentDtoValidator())
            .When(x => x.Payment != null);
        
        RuleFor(x => x.Transport)
            .SetValidator(new TransportDtoValidator())
            .When(x => x.Transport != null);
        
        RuleForEach(x => x.Payloads)
            .SetValidator(new PayloadDtoValidator())
            .When(x => x.Payloads != null);
        
        RuleForEach(x => x.RoutePoints)
            .SetValidator(new RoutePointDtoValidator())
            .When(x => x.RoutePoints != null);
    }
}