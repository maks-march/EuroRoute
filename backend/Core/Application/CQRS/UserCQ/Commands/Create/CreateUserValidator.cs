using FluentValidation;

namespace Application.CQRS.UserCQ.Commands.Create;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(command => command.Surname)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(command => command.Surname)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(command => command.Surname)
            .NotEmpty()
            .MaximumLength(256);
    }
}