using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.DAL;

public class TicketRepository : IRepositoryCRU<Ticket> {

    private readonly ApplicationDbContext _context;

    public TicketRepository(ApplicationDbContext context) {
        _context = context;
    }

    public List<Ticket> GetList(Func<Ticket, bool>? whereFunction) {
        if (whereFunction is null) {
            throw new ArgumentNullException();
        }
        
        return _context.Tickets
            .Include(t => t.Project)
            .Include(t => t.OwnerUser)
            .Include(t => t.AssignedToUser)
            .Include(t => t.TicketType)
            .Include(t => t.TicketPriority)
            .Include(t => t.TicketStatus)
            .Where(whereFunction).ToList();
    }

    public Ticket Get(Func<Ticket, bool>? firstFunction) {
        if (firstFunction is null) {
            throw new ArgumentNullException();
        }
        return _context.Tickets
            .Include(t => t.Project)
            .Include(t => t.OwnerUser)
            .Include(t => t.AssignedToUser)
            .Include(t => t.TicketType)
            .Include(t => t.TicketPriority)
            .Include(t => t.TicketStatus)
            .First(firstFunction);
    }

    public void Create(Ticket? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.Tickets.Add(entity);
    }

    public void Save() {
        _context.SaveChanges();
    }

    public void Update(Ticket? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.Tickets.Update(entity);
    }
}