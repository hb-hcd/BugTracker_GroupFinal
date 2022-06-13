namespace BugTracker.Models;

public class TicketPriority {
    public int Id { get; set; }
    public TicketPriorityName Name { get; set; }
}

public enum TicketPriorityName {
    High = 1, 
    Medium = 2, 
    Low = 3,
} 
