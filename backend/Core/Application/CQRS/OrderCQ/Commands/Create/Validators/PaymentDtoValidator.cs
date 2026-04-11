using FluentValidation;

namespace Application.CQRS.OrderCQ.Commands.Create.Validators;

public class PaymentDtoValidator : AbstractValidator<PaymentCommandDto>
{
    public PaymentDtoValidator()
    {
        RuleForEach(x => new []{x.TaxedByCard, x.NotTaxedByCard, x.ByCash})
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