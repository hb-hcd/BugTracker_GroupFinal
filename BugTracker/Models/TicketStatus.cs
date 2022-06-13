namespace BugTracker.Models;

public class TicketStatus {
    public int Id { get; set; }
    public TicketStatusName Name { get; set; }
}

public enum TicketStatusName {
    Unassigned = 1,
    Assigned = 2,
    InProgress = 3,
    Completed = 4
}
