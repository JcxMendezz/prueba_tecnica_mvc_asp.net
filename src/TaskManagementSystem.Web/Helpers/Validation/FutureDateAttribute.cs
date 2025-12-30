using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Helpers.Validation;

/// <summary>
/// Validation attribute that ensures a date is today or in the future.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class FutureDateAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FutureDateAttribute"/> class.
    /// </summary>
    public FutureDateAttribute()
        : base("La fecha debe ser igual o posterior a hoy")
    {
    }

    /// <summary>
    /// Determines whether the specified value is valid.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A validation result.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Null is valid (use [Required] for mandatory dates)
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is DateTime dateValue)
        {
            if (dateValue.Date >= DateTime.Today)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(
                ErrorMessage ?? "La fecha de vencimiento no puede ser anterior a hoy",
                new[] { validationContext.MemberName ?? string.Empty });
        }

        return new ValidationResult("Valor de fecha inválido");
    }
}
