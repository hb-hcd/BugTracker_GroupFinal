namespace BugTracker.Models;

public class TicketNotification {
    public int Id { get; set; }
    public string? Description { get; set; }
    public virtual Ticket? Ticket { get; set; }
    public int TicketId { get; set; }
    public virtual User? User { get; set; }
    public string? UserId { get; set; }
}
