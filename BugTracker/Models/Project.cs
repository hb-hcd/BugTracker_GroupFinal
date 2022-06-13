namespace BugTracker.Models;

public class Project {
    public int Id { get; set; }
    public string? Name { get; set; }
    public virtual User? User { get; set; }
    public string? UserId { get; set; }
    
    public virtual ICollection<ProjectUser>? ProjectUsers { get; set; } = null;
    public virtual ICollection<Ticket>? Tickets { get; set; } = null;
}
