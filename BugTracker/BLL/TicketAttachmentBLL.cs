using BugTracker.DAL;
using BugTracker.Models;

namespace BugTracker.BLL; 

public class TicketAttachmentBll {
    private readonly IRepositoryCRUD<TicketAttachment> _ticketAttachmentRepository;
    public TicketAttachmentBll(IRepositoryCRUD<TicketAttachment> ticketAttachmentRepository) {
        _ticketAttachmentRepository = ticketAttachmentRepository;
    }

    public virtual List<TicketAttachment> GetTicketAttachments(int ticketId) {
        return _ticketAttachmentRepository.GetList(t => t.TicketId == ticketId);
    }
    
    public virtual TicketAttachment GetTicketAttachment(int id) {
        return _ticketAttachmentRepository.Get(ta => ta.Id == id);
    }
    
    public virtual void CreateAttachment(TicketAttachment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }
        
        _ticketAttachmentRepository.Create(entity);
    }

    public virtual void UpdateAttachment(TicketAttachment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }
        
        _ticketAttachmentRepository.Update(entity);
    }

    public virtual void DeleteAttachment(TicketAttachment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }
        _ticketAttachmentRepository.Delete(entity);
    }
    
    public void Save() {
        _ticketAttachmentRepository.Save();
    }
}
