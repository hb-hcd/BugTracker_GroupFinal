namespace BugTracker.Models;

public class Ticket {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
    public virtual Project? Project { get; set; }
    public int ProjectId { get; set; }
    public virtual TicketType? TicketType { get; set; }
    public int TicketTypeId { get; set; }
    public virtual TicketStatus? TicketStatus { get; set; }
    public int TicketStatusId { get; set; }
    public virtual TicketPriority? TicketPriority { get; set; }
    public int TicketPriorityId { get; set; }
    
    //[ForeignKey("OwnedTickets")]
    public virtual User? OwnerUser { get; set; }
    public string? OwnerUserId { get; set; }
    
    //[ForeignKey("AssignedTickets")]
    public virtual User? AssignedToUser { get; set; }
    public string? AssignedToUserId { get; set; }

    public virtual ICollection<TicketAttachment>? TicketAttachments { get; set; } = null;
    public virtual ICollection<TicketComment>? TicketComments { get; set; } = null;
    public virtual ICollection<TicketHistory>? TicketHistories { get; set; } = null;
    public virtual ICollection<TicketNotification>? TicketNotifications { get; set; } = null;
    
    public Ticket Copy() {
        return new() {
            Id = Id,
            Title = Title,
            Created = Created,
            Updated = Updated,
            Description = Description,
            ProjectId = ProjectId,
            TicketTypeId = TicketTypeId,
            TicketPriorityId = TicketPriorityId,
            TicketStatusId = TicketStatusId,
            OwnerUserId = OwnerUserId,  
            AssignedToUserId = AssignedToUserId
        };
    }
}
