using FluentValidation;

namespace Application.Directors.Commands;

public class UpdateDirectorCommandValidator : AbstractValidator<UpdateDirectorCommand>
{
    public UpdateDirectorCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.");

        RuleFor(x => x.Bio)
            .NotEmpty().WithMessage("Bio is required.")
            .MaximumLength(1000).WithMessage("Bio must not exceed 1000 characters.")
            .MinimumLength(10).WithMessage("Bio must be at least 10 characters long.");

        RuleFor(x => x.DirectorId)
            .NotEmpty().WithMessage("DirectorId is required.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth Date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Birth Date cannot be in the future.");
    }
}