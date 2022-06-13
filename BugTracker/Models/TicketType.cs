namespace BugTracker.Models;

public class TicketType {
    public int Id { get; set; }
    public TicketTypeName Name { get; set; }
}

public enum TicketTypeName {
    Incident = 1,
    ServiceRequest = 2,
    InformationRequest = 3
}
