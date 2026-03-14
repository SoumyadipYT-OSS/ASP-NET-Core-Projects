
namespace LibraryManagement.Api.Models;


public class Members 
{
    public int MemberId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateTime MembershipDate { get; set; }
}