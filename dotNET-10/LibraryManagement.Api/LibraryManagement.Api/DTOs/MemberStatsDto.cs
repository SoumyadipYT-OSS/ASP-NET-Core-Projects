namespace LibraryManagement.Api.DTOs;

public class MemberStatsDto 
{
    public int MemberId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalBorrowed { get; set; }
    public int CurrentlyBorrowed { get; set; }
    public int OverdueCount { get; set; }
}