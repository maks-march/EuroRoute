using Domain.Enums;
using FluentValidation;
using WebApi.Extensions;

namespace Application.CQRS.OrderCQ.Commands.Create.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.")
            .GreaterThan(DateTime.Now.ToDateOnly())
            .WithMessage("Start date must be in the future.");
        
        RuleFor(x => x.Status)
            .MaximumLength(20)
            .WithMessage("Status must be 20 characters or fewer.")
            .Must(x => Enum.IsDefined(typeof(OrderStatus), x))
            .WithMessage("Status must be one of the following.");

        RuleFor(x => x.SpecNumber)
            .InclusiveBetween(100, 100000)
            .WithMessage("SpecNumber must be greater than 100.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Author ID is required.");
        
        RuleFor(x => x.About)
            .MaximumLength(200)
            .WithMessage("About must be 200 characters or fewer.");
        
        RuleFor(x => x.Payloads)
            .NotEmpty().WithMessage("At least one payload is required.");
        
        RuleFor(x => x.RoutePoints)
            .NotEmpty()
            .WithMessage("At least two route points are required (start and end).")
            .Must(points => points.Count >= 2)
            .WithMessage("At least two route points are required (start and end).");


        RuleFor(x => x.Payment)
            .NotNull()
            .SetValidator(new PaymentDtoValidator());
        RuleFor(x => x.Transport)
            .NotNull()
            .SetValidator(new TransportDtoValidator());
        RuleForEach(x => x.Payloads)
            .SetValidator(new PayloadDtoValidator());
        RuleForEach(x => x.RoutePoints)
            .SetValidator(new RoutePointDtoValidator());
    }
}