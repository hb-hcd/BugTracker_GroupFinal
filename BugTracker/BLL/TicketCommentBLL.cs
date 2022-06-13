using BugTracker.DAL;
using BugTracker.Models;

namespace BugTracker.BLL; 

public class TicketCommentBll {
    private readonly IRepositoryCRUD<TicketComment> _ticketCommentRepository;
    public TicketCommentBll(IRepositoryCRUD<TicketComment> ticketCommentRepository) {
        _ticketCommentRepository = ticketCommentRepository;
    }

    public virtual TicketComment Get(int? id)
    {
        if (id is null) { throw new ArgumentNullException(); }

        TicketComment? comment = _ticketCommentRepository.Get(c => c.Id == id);

        if(comment is null) { throw new ArgumentException(); }
        
        return comment;
    }

    public virtual void CreateComment(TicketComment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }
        
        _ticketCommentRepository.Create(entity);
        _ticketCommentRepository.Save();
    }

    public virtual void UpdateComment(TicketComment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }
        
        _ticketCommentRepository.Update(entity);
        _ticketCommentRepository.Save();
    }

    public virtual void DeleteComment(TicketComment? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }
        _ticketCommentRepository.Delete(entity);
        _ticketCommentRepository.Save();
    }
    
    public void Save() {
        _ticketCommentRepository.Save();
    }

    public virtual List<TicketComment> GetTicketCommentsByTicketId(int? id)
    {
        if(id is null) { throw new ArgumentNullException(); }
        return _ticketCommentRepository.GetList(c => c.TicketId == id).ToList();
    }

    public virtual TicketComment GetCommentById(int? id)
    {
        if(id is null) { throw new ArgumentNullException(); }
        var comment = _ticketCommentRepository.Get(c => c.Id == id);
        if(comment is null) { throw new ArgumentException(); }
        return comment;
    }
}
