using FluentValidation;
using BhelInspectionApi.Models.DTOs;

namespace BhelInspectionApi.Validators
{
    public class CreateInspectionFormValidator : AbstractValidator<CreateInspectionFormDto>
    {
        public CreateInspectionFormValidator()
        {
            RuleFor(x => x.FormId)
                .NotEmpty().WithMessage("Form ID is required")
                .MaximumLength(50).WithMessage("Form ID cannot exceed 50 characters");

            RuleFor(x => x.Revision)
                .NotEmpty().WithMessage("Revision is required")
                .MaximumLength(20).WithMessage("Revision cannot exceed 20 characters");

            RuleFor(x => x.JobNumber)
                .NotEmpty().WithMessage("Job Number is required")
                .MaximumLength(50).WithMessage("Job Number cannot exceed 50 characters")
                .Matches(@"^[A-Z0-9-]+$").WithMessage("Job Number can only contain uppercase letters, numbers, and hyphens");

            RuleFor(x => x.Customer)
                .NotEmpty().WithMessage("Customer is required")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters");

            RuleFor(x => x.Frame)
                .NotEmpty().WithMessage("Frame is required")
                .MaximumLength(50).WithMessage("Frame cannot exceed 50 characters");

            RuleFor(x => x.Component)
                .NotEmpty().WithMessage("Component is required")
                .MaximumLength(100).WithMessage("Component cannot exceed 100 characters");

            RuleFor(x => x.PartNumber)
                .NotEmpty().WithMessage("Part Number is required")
                .MaximumLength(50).WithMessage("Part Number cannot exceed 50 characters")
                .Matches(@"^[A-Z0-9-]+$").WithMessage("Part Number can only contain uppercase letters, numbers, and hyphens");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(10000).WithMessage("Quantity cannot exceed 10,000");

            RuleFor(x => x.Operator)
                .NotEmpty().WithMessage("Operator is required")
                .MaximumLength(100).WithMessage("Operator name cannot exceed 100 characters");

            RuleFor(x => x.InspectionDate)
                .NotEmpty().WithMessage("Inspection Date is required")
                .GreaterThanOrEqualTo(new DateTime(2020, 1, 1)).WithMessage("Inspection Date cannot be before 2020")
                .LessThanOrEqualTo(DateTime.Today.AddDays(30)).WithMessage("Inspection Date cannot be more than 30 days in the future");

            RuleFor(x => x.InstrumentId)
                .NotEmpty().WithMessage("Instrument ID is required")
                .MaximumLength(50).WithMessage("Instrument ID cannot exceed 50 characters");

            RuleFor(x => x.CalibrationDueDate)
                .NotEmpty().WithMessage("Calibration Due Date is required")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Calibration Due Date cannot be in the past");

            RuleFor(x => x.InspectedBy)
                .NotEmpty().WithMessage("Inspected By is required")
                .MaximumLength(100).WithMessage("Inspected By cannot exceed 100 characters");

            RuleFor(x => x.ReviewedBy)
                .NotEmpty().WithMessage("Reviewed By is required")
                .MaximumLength(100).WithMessage("Reviewed By cannot exceed 100 characters");

            RuleForEach(x => x.BladeMeasurements).SetValidator(new CreateBladeMeasurementValidator());
        }
    }

    public class CreateBladeMeasurementValidator : AbstractValidator<CreateBladeMeasurementDto>
    {
        public CreateBladeMeasurementValidator()
        {
            RuleFor(x => x.BladeNumber)
                .GreaterThan(0).WithMessage("Blade Number must be greater than 0")
                .LessThanOrEqualTo(90).WithMessage("Blade Number cannot exceed 90");

            RuleFor(x => x.PartNumber)
                .MaximumLength(50).WithMessage("Part Number cannot exceed 50 characters")
                .Matches(@"^[A-Z0-9-]*$").WithMessage("Part Number can only contain uppercase letters, numbers, and hyphens")
                .When(x => !string.IsNullOrEmpty(x.PartNumber));

            RuleFor(x => x.SerialNumber)
                .MaximumLength(50).WithMessage("Serial Number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.SerialNumber));

            // Measurement validation rules
            ValidateMeasurement(x => x.D1, "D1");
            ValidateMeasurement(x => x.D2, "D2");
            ValidateMeasurement(x => x.D3, "D3");
            ValidateMeasurement(x => x.D4, "D4");
            ValidateMeasurement(x => x.D5, "D5");
            ValidateMeasurement(x => x.D6, "D6");
            ValidateMeasurement(x => x.D7, "D7");
            ValidateMeasurement(x => x.D8, "D8");
            ValidateMeasurement(x => x.T1, "T1");
            ValidateMeasurement(x => x.T2, "T2");
            ValidateMeasurement(x => x.T3, "T3");
            ValidateMeasurement(x => x.T4, "T4");
            ValidateMeasurement(x => x.T5, "T5");
            ValidateMeasurement(x => x.E1, "E1");
            ValidateMeasurement(x => x.E2, "E2");
            ValidateMeasurement(x => x.E3, "E3");
        }

        private void ValidateMeasurement(System.Linq.Expressions.Expression<Func<CreateBladeMeasurementDto, decimal?>> expression, string measurementName)
        {
            RuleFor(expression)
                .GreaterThanOrEqualTo(0.000m).WithMessage($"{measurementName} must be greater than or equal to 0.000")
                .LessThanOrEqualTo(0.100m).WithMessage($"{measurementName} must be less than or equal to 0.100")
                .ScalePrecision(4, 8).WithMessage($"{measurementName} can have at most 4 decimal places")
                .When(x => expression.Compile()(x).HasValue);
        }
    }
}