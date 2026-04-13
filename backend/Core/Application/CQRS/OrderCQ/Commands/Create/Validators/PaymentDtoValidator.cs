using Domain.Enums;
using FluentValidation;

namespace Application.CQRS.OrderCQ.Commands.Create.Validators;

public class PaymentDtoValidator : AbstractValidator<PaymentCommandDto>
{
    public PaymentDtoValidator()
    {
        RuleFor(x => x.PaymentType)
            .MaximumLength(20)
            .WithMessage("Payment type must be 20 characters or fewer.")
            .Must(x => Enum.IsDefined(typeof(PaymentType), x))
            .WithMessage("Payment type must be one of the following.");
        
        RuleFor(x => x.TaxedByCard)
            .Must(pay => pay >= 0 && pay < double.MaxValue)
            .WithMessage("Payment must be between 0 and 2^32.");
        RuleFor(x => x.NotTaxedByCard)
            .Must(pay => pay >= 0 && pay < double.MaxValue)
            .WithMessage("Payment must be between 0 and 2^32.");
        RuleFor(x => x.ByCash)
            .Must(pay => pay >= 0 && pay < double.MaxValue)
            .WithMessage("Payment must be between 0 and 2^32.");
        
        RuleFor(x => x.PaymentAfterDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Payment delay must be positive.");
        
        RuleFor(x => x.Prepayment)
            .Must(perc => perc is >= 0 and <= 100)
            .WithMessage("Prepayment percent must be between 0 and 100.");
    }
}