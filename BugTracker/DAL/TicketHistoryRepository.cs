using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.DAL;

public class TicketHistoryRepository : IRepositoryCR<TicketHistory> {

    private readonly ApplicationDbContext _context;

    public TicketHistoryRepository(ApplicationDbContext context) {
        _context = context;
    }

    public List<TicketHistory> GetList(Func<TicketHistory, bool>? whereFunction) {
        if (whereFunction is null) {
            throw new ArgumentNullException();
        }
        
        return _context.TicketHistories.Where(whereFunction).ToList();
    }

    public TicketHistory Get(Func<TicketHistory, bool>? firstFunction) {
        if (firstFunction is null) {
            throw new ArgumentNullException();
        }
        return _context.TicketHistories.First(firstFunction);
    }

    public void Create(TicketHistory? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.TicketHistories.Add(entity);
    }

    public void Save() {
        _context.SaveChanges();
    }
}
