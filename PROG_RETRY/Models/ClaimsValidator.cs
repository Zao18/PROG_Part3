using FluentValidation;
using PROG_Part_2.Models;

public class ClaimsValidator : AbstractValidator<Claims>
{
    public ClaimsValidator()
    {
        RuleFor(x => x.HoursWorked).GreaterThan(0).WithMessage("Hours worked must be greater than zero.");
        RuleFor(x => x.HourlyRate).GreaterThan(0).WithMessage("Hourly rate must be greater than zero.");
        RuleFor(x => x.TotalPayment).GreaterThan(0).When(x => x.TotalPayment.HasValue).WithMessage("Total payment must be positive.");
        RuleFor(x => x.DocumentName).NotEmpty().WithMessage("Document must be uploaded.");
    }
}
