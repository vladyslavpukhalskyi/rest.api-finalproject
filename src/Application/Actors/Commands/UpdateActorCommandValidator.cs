using FluentValidation;

namespace Application.Actors.Commands;

public class UpdateActorCommandValidator : AbstractValidator<UpdateActorCommand>
{
    public UpdateActorCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("ActorId is required.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.");
        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("BirthDate is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("BirthDate cannot be in the future.");
    }
}