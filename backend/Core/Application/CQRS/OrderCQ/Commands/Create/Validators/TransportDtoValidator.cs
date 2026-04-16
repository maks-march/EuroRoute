using FluentValidation;

namespace Application.CQRS.OrderCQ.Commands.Create.Validators;

public class TransportDtoValidator : AbstractValidator<TransportCreateCommandDto>
{
    public TransportDtoValidator()
    {
        RuleFor(x => x.BodyType)
            .NotEmpty()
            .WithMessage("Body type is required.")
            .ForEach(type => type.MaximumLength(20))
            .WithMessage("Body type must be 20 characters or fewer.");

        RuleFor(x => x.Vehicles)
            .GreaterThan(0).WithMessage("Number of vehicles must be at least 1.");
        
        RuleFor(x => x.LoadType)
            .ForEach(type => type.MaximumLength(20))
            .WithMessage("Load type must be 20 characters or fewer.");
        
        RuleFor(x => x.UnloadType)
            .ForEach(type => type.MaximumLength(20))
            .WithMessage("Unload type must be 20 characters or fewer.");
        
        // ADR - класс опасности, обычно от 1 до 9. Убедимся, что он в разумных пределах.
        RuleFor(x => x.Adr)
            .InclusiveBetween(1, 9)
            .WithMessage("ADR class must be between 1 and 9.");

        // Логическая проверка температурного режима
        When(x => x.TemperatureFrom.HasValue && x.TemperatureTo.HasValue, () =>
        {
            RuleFor(x => x.TemperatureTo)
                .GreaterThanOrEqualTo(x => x.TemperatureFrom)
                .WithMessage("'Temperature To' must be greater than or equal to 'Temperature From'.");
        });
    }
}