using FluentValidation;

namespace Application.CQRS.OrderCQ.Commands.Update.Validators;

public class RoutePointDtoValidator : AbstractValidator<RoutePointUpdateCommand>
{
    public RoutePointDtoValidator()
    {
        RuleFor(x => x.City)
            .MaximumLength(100)
            .WithMessage("City name cannot exceed 100 characters.");

        RuleFor(x => x.Address)
            .MaximumLength(100)
            .WithMessage("Address cannot exceed 100 characters.");

        RuleFor(x => x.Date)
            .GreaterThan(DateTime.UtcNow.AddDays(-1))
            .When(x => x.Date != null)
            .WithMessage("Date cannot be in the past.");

        RuleFor(x => x.LoadTimeEnd)
            .GreaterThanOrEqualTo(x => x.LoadTimeStart)
            .When(x => x.LoadTimeEnd != null)
            .WithMessage("'Load time end' must be after or the same as 'Load time start'.");
        
        RuleFor(x => x.LoadTimeStart)
            .LessThanOrEqualTo(x => x.LoadTimeEnd)
            .When(x => x.LoadTimeStart != null)
            .WithMessage("'Load time end' must be after or the same as 'Load time start'.");
    }
}