using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.Validations;

public class PublishYearValidation 
{
    public static ValidationResult ValidatePublishedYear(int year, ValidationContext context) 
    {
        int currentYear = DateTime.Now.Year;
        if (year < 1500 || year > currentYear) 
        {
            return new ValidationResult($"Published year must be between 1500 and {currentYear}");
        }
        return ValidationResult.Success!;
    }
}