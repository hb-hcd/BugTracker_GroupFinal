namespace BugTracker.Models;

public class TicketComment {
    public int Id { get; set; }
    public string? Comment { get; set; }
    public DateTime Created { get; set; }
    public virtual Ticket? Ticket { get; set; }
    public int TicketId { get; set; }
    public virtual User? User { get; set; }
    public string? UserId { get; set; }
}
