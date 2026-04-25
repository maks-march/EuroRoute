using Domain.Enums;
using FluentValidation;

namespace Application.CQRS.OrderCQ.Commands.Update.Validators;

public class PaymentDtoValidator : AbstractValidator<PaymentUpdateCommand?>
{
    public PaymentDtoValidator()
    {
        RuleFor(x => x)
            .NotNull();
        
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        RuleFor(x => x.PaymentType)
            .MaximumLength(20)
            .WithMessage("Payment type must be 20 characters or fewer.")
            .Must(x => Enum.IsDefined(typeof(PaymentType), x))
            .When(x => x?.PaymentType != null)
            .WithMessage("Payment type must be one of the following.");
        
        RuleFor(x => x.TaxedByCard)
            .Must(pay => pay is >= 0 and < double.MaxValue)
            .When(x => x?.TaxedByCard != null)
            .WithMessage("Payment must be between 0 and 2^32.");
        
        RuleFor(x => x.NotTaxedByCard)
            .Must(pay => pay is >= 0 and < double.MaxValue)
            .When(x => x?.NotTaxedByCard != null)
            .WithMessage("Payment must be between 0 and 2^32.");
        
        RuleFor(x => x.ByCash)
            .Must(pay => pay is >= 0 and < double.MaxValue)
            .When(x => x?.ByCash != null)
            .WithMessage("Payment must be between 0 and 2^32.");
        
        RuleFor(x => x.PaymentAfterDays)
            .GreaterThanOrEqualTo(0)
            .When(x => x?.PaymentAfterDays != null)
            .WithMessage("Payment delay must be positive.");
        
        RuleFor(x => x.Prepayment)
            .Must(perc => perc is >= 0 and <= 100)
            .When(x => x.Prepayment != null)
            .WithMessage("Prepayment percent must be between 0 and 100.");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}