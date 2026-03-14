
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.DTOs;


public class BorrowRequestDto 
{
    [Required(ErrorMessage = "BookId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "BookId must be a positive integer")]
    public int BookId { get; set; }
    [Required(ErrorMessage = "MemberId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "MemberId must be a positive integer")]
    public int MemberId { get; set; }
    [Required(ErrorMessage = "DueDate is required")]
    [DataType(DataType.Date, ErrorMessage = "DueDate must be a valid date")]
    public DateTime DueDate { get; set; }
}

public class BorrowResponseDto 
{
    public bool Success { get; set; }
    [Required]
    [StringLength(200, ErrorMessage = "Message cannot exceed 200 characters")]
    public string Message { get; set; } = "";
}