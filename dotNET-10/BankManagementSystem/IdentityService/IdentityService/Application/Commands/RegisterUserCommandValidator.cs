using FluentValidation;

namespace IdentityService.Application.Commands;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Must(ContainUpperCase).WithMessage("Password must contain at least one uppercase letter")
            .Must(ContainLowerCase).WithMessage("Password must contain at least one lowercase letter")
            .Must(ContainDigit).WithMessage("Password must contain at least one digit")
            .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Must(BeAtLeast18YearsOld).WithMessage("User must be at least 18 years old");
    }

    private static bool ContainUpperCase(string password) => password.Any(char.IsUpper);
    private static bool ContainLowerCase(string password) => password.Any(char.IsLower);
    private static bool ContainDigit(string password) => password.Any(char.IsDigit);
    private static bool ContainSpecialCharacter(string password) => 
        password.Any(c => !char.IsLetterOrDigit(c));

    private static bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }
}
