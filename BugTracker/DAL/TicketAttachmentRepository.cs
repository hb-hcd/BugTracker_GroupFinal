using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.DAL;

public class TicketAttachmentRepository : IRepositoryCRUD<TicketAttachment> {

    private readonly ApplicationDbContext _context;

    public TicketAttachmentRepository(ApplicationDbContext context) {
        _context = context;
    }

    public List<TicketAttachment> GetList(Func<TicketAttachment, bool>? whereFunction) {
        if (whereFunction is null) {
            throw new ArgumentNullException();
        }
        
        return _context.TicketAttachments.Where(whereFunction).ToList();
    }

    public TicketAttachment Get(Func<TicketAttachment, bool>? firstFunction) {
        if (firstFunction is null) {
            throw new ArgumentNullException();
        }
        return _context.TicketAttachments.First(firstFunction);
    }

    public void Create(TicketAttachment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.TicketAttachments.Add(entity);
    }

    public void Save() {
        _context.SaveChanges();
    }

    public void Update(TicketAttachment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.TicketAttachments.Update(entity);
    }

    public void Delete(TicketAttachment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.TicketAttachments.Remove(entity);
    }
}
