using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models;

public class Student {
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name should be between 3 to 50 characters long.")]
    public string? Name { get; set; }

    [Required]
    [RegularExpression(@"^\d{10,12}$", ErrorMessage = "Invalid Registration Number! The Registration Number should be 10-12 digits long.")]
    public long? RegdNo { get; set; }

    [Required]
    public string? Course { get; set; }

    [Required]
    public string? Specilization { get; set; }

    [Required]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Phone Number! The Phone Number should be 10 digits long.")]
    public long? PhNo { get; set; }
}