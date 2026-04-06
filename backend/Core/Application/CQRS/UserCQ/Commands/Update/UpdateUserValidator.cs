using Application.CQRS.UserCQ.Commands.Update;
using FluentValidation;

namespace Application.CQRS.UserCQ.Commands.Create;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(command => command.Surname)
            .NotEmpty()
            .MaximumLength(50);
    }
}