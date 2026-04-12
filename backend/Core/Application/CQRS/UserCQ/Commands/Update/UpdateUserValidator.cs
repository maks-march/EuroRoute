using FluentValidation;

namespace Application.CQRS.UserCQ.Commands.Update;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(command => command.Name)
            .MaximumLength(50);
        RuleFor(command => command.Surname)
            .MaximumLength(50);
    }
}