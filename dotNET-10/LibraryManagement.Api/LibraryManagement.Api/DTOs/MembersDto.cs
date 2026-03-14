using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.DTOs;

public class CreateMemberRequest 
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = "";

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string Phone { get; set; } = "";
}

public class MembersDto 
{
    [Key]
    public int MemberId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = "";

    [StringLength(20)]
    public string Phone { get; set; } = "";

    [DataType(DataType.Date)]
    public DateTime MembershipDate { get; set; }
}

public class MemberHistoryDto 
{
    public int TransactionId { get; set; }

    [Required]
    public string BookTitle { get; set; } = "";

    [DataType(DataType.Date)]
    public DateTime BorrowDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? ReturnDate { get; set; }

    [DataType(DataType.Currency)]
    public decimal FineAmount { get; set; }
}
