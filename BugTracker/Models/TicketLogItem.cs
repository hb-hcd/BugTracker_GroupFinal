namespace BugTracker.Models;

public class TicketLogItem {
    public int Id { get; set; }
    public virtual TicketHistory? TicketHistory { get; set; }
    public int TicketHistoryId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
    public string? Project { get; set; }
    public string? TicketType { get; set; }
    public string? TicketStatus { get; set; }
    public string? TicketPriority { get; set; }
    public string? OwnerUser { get; set; }
    public string? AssignedToUser { get; set; }
}
