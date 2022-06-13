using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.DAL;

public class TicketNotificationRepository : IRepositoryCRUD<TicketNotification> {

    private readonly ApplicationDbContext _context;

    public TicketNotificationRepository(ApplicationDbContext context) {
        _context = context;
    }

    public List<TicketNotification> GetList(Func<TicketNotification, bool>? whereFunction) {
        if (whereFunction is null) {
            throw new ArgumentNullException();
        }
        
        return _context.TicketNotifications.Where(whereFunction).ToList();
    }

    public TicketNotification Get(Func<TicketNotification, bool>? firstFunction) {
        if (firstFunction is null) {
            throw new ArgumentNullException();
        }
        return _context.TicketNotifications.First(firstFunction);
    }

    public void Create(TicketNotification? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.TicketNotifications.Add(entity);
    }

    public void Save() {
        _context.SaveChanges();
    }

    public void Update(TicketNotification? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.TicketNotifications.Update(entity);
    }

    public void Delete(TicketNotification? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.TicketNotifications.Remove(entity);
    }
}