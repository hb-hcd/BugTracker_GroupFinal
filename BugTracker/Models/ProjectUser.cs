namespace BugTracker.Models;

public class ProjectUser {
    public int Id { get; set; }
    public virtual Project? Project { get; set; }
    public int ProjectId { get; set; }
    public virtual User? User { get; set; }
    public string? UserId { get; set; }
}
