using FluentValidation;

namespace Application.CQRS.OrderCQ.Commands.Create.Validators;

public class PayloadDtoValidator : AbstractValidator<PayloadCommandDto>
{
    public PayloadDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Payload's name must be 100 characters or fewer.");
        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0.");
        RuleFor(x => x.Volume)
            .GreaterThan(0)
            .WithMessage("Volume must be greater than 0.");
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0.");
    }
}