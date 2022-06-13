using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models;

public class TicketHistory {
    public int Id { get; set; }
    public virtual Ticket? Ticket { get; set; }
    public int TicketId { get; set; }
    public string? Properties { get; set; }
    public DateTime DateOfChange { get; set; } = DateTime.Now;
    [ForeignKey("TicketHistory")]
    public virtual TicketLogItem? TicketLogItem { get; set; }
    public int TicketLogItemId { get; set; }
} 
