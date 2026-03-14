
namespace LibraryManagement.Api.Models;


public class Books 
{
    public int BookId { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string ISBN { get; set; } = "";
    public int PublishedYear { get; set; }
    public string Category { get; set; } = "";
    public int CopiesAvailable { get; set; }
}