
namespace LibraryManagement.Api.Models;


public class BorrowTransactions 
{
    public int TransactionId { get; set; } 
    public int BookId { get; set; } 
    public int MemberId { get; set; } 
    public DateTime BorrowDate { get; set; } 
    public DateTime DueDate { get; set; } 
    public DateTime? ReturnDate { get; set; } 
    public decimal FineAmount { get; set; }

    // Navigation properties
    public Books? Book { get; set; }
    public Members? Member { get; set; }
}