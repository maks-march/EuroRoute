using FluentValidation;

namespace Application.CQRS.OrderCQ.Commands.Create.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.")
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Start date must be in the future.");

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