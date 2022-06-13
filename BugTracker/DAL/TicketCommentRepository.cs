using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.DAL;

public class TicketCommentRepository : IRepositoryCRUD<TicketComment> {

    private readonly ApplicationDbContext _context;

    public TicketCommentRepository(ApplicationDbContext context) {
        _context = context;
    }

    public List<TicketComment> GetList(Func<TicketComment, bool>? whereFunction) {
        if (whereFunction is null) {
            throw new ArgumentNullException();
        }
        
        return _context.TicketComments.Where(whereFunction).ToList();
    }

    public TicketComment Get(Func<TicketComment, bool>? firstFunction) {
        if (firstFunction is null) {
            throw new ArgumentNullException();
        }
        return _context.TicketComments.First(firstFunction);
    }

    public void Create(TicketComment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.TicketComments.Add(entity);
    }

    public void Save() {
        _context.SaveChanges();
    }

    public void Update(TicketComment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.TicketComments.Update(entity);
    }

    public void Delete(TicketComment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.TicketComments.Remove(entity);
    }
}