using LibraryManagement.Api.Validations;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.DTOs;

public class CreateBookRequest 
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = "";
    [Required(ErrorMessage = "Author is required")]
    [StringLength(100, ErrorMessage = "Author cannot exceed 100 characters")]
    public string Author { get; set; } = "";
    [Required(ErrorMessage = "ISBN is required")]
    [StringLength(50, ErrorMessage = "ISBN cannot exceed 50 characters")]
    public string ISBN { get; set; } = "";
    [CustomValidation(typeof(PublishYearValidation), nameof(PublishYearValidation.ValidatePublishedYear))]
    public int PublishedYear { get; set; }
    [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
    public string Category { get; set; } = "";
    [Range(0, int.MaxValue, ErrorMessage = "Copies must be non-negative")]
    public int CopiesAvailable { get; set; }
}

public class BooksDto 
{
    [Key]
    public int BookId { get; set; }
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = "";
    [Required]
    [StringLength(100)]
    public string Author { get; set; } = "";
    [Required]
    [StringLength(50)]
    public string Category { get; set; } = "";
    [Range(0, int.MaxValue)]
    public int CopiesAvailable { get; set; }
}
