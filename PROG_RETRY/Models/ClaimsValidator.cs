using FluentValidation;
using PROG_Part_2.Models;

public class ClaimValidator : AbstractValidator<Claims>
{
    public ClaimValidator()
    {
        RuleFor(x => x.HoursWorked).GreaterThan(0).WithMessage("Hours worked must be greater than zero.");
        RuleFor(x => x.HourlyRate).GreaterThan(0).WithMessage("Hourly rate must be greater than zero.");
    }
}
