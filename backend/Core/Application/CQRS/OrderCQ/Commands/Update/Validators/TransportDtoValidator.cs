using FluentValidation;

namespace Application.CQRS.OrderCQ.Commands.Update.Validators;

public class TransportDtoValidator : AbstractValidator<TransportUpdateCommand>
{
    public TransportDtoValidator()
    {
        RuleFor(x => x.BodyType)
            .ForEach(type => type.MaximumLength(20))
            .When(x => x.BodyType != null)
            .WithMessage("Body type must be 20 characters or fewer.");

        RuleFor(x => x.Vehicles)
            .GreaterThan(0)
            .When(x => x.Vehicles != null)
            .WithMessage("Number of vehicles must be at least 1.");
        
        RuleFor(x => x.LoadType)
            .ForEach(type => type.MaximumLength(20))
            .WithMessage("Load type must be 20 characters or fewer.");
        
        RuleFor(x => x.UnloadType)
            .ForEach(type => type.MaximumLength(20))
            .WithMessage("Unload type must be 20 characters or fewer.");
        
        RuleFor(x => x.Adr)
            .InclusiveBetween(1, 9)
            .When(x => x.Adr != null)
            .WithMessage("ADR class must be between 1 and 9.");

        
        RuleFor(x => x.TemperatureTo)
            .GreaterThanOrEqualTo(x => x.TemperatureFrom)
            .When(x => x.TemperatureTo != null && x.TemperatureFrom.HasValue)
            .WithMessage("'Temperature To' must be greater than or equal to 'Temperature From'.");
        
        RuleFor(x => x.TemperatureFrom)
            .LessThanOrEqualTo(x => x.TemperatureTo)
            .When(x => x.TemperatureFrom != null && x.TemperatureTo.HasValue)
            .WithMessage("'Temperature To' must be greater than or equal to 'Temperature From'.");
    }
}