using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Models;

public class User : IdentityUser {
    public virtual ICollection<Project>? Projects { get; set; } = null;
    public virtual ICollection<ProjectUser>? ProjectUsers { get; set; } = null;
    
    [InverseProperty("AssignedToUser")]
    public virtual ICollection<Ticket>? AssignedTickets { get; set; } = null;
    
    [InverseProperty("OwnerUser")]
    public virtual ICollection<Ticket>? OwnedTickets { get; set; } = null;

    public virtual ICollection<TicketAttachment>? TicketAttachments { get; set; } = null;
    public virtual ICollection<TicketComment>? TicketComments { get; set; } = null;
    public virtual ICollection<TicketHistory>? TicketHistories { get; set; } = null;
    public virtual ICollection<TicketNotification>? TicketNotifications { get; set; } = null;
}
    