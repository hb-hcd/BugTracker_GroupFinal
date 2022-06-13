using BugTracker.DAL;
using BugTracker.Models;

namespace BugTracker.BLL; 

public class TicketBusinessLogicLayer {
    private readonly IRepositoryCRU<Ticket> _ticketRepository;
    private readonly IRepositoryCR<TicketLogItem> _ticketLogItemRepository;
    private readonly IRepositoryCR<TicketHistory> _ticketHistoryRepository;

    public TicketBusinessLogicLayer(IRepositoryCRU<Ticket> ticketRepository, IRepositoryCR<TicketLogItem> ticketLogItemRepository, IRepositoryCR<TicketHistory> ticketHistoryRepository) {
        _ticketRepository = ticketRepository;
        _ticketLogItemRepository = ticketLogItemRepository;
        _ticketHistoryRepository = ticketHistoryRepository;
    }

    public virtual List<Ticket> GetAll(int index = 0, int limit=10) {
        return _ticketRepository.GetList(_ => true).GetRange(index, limit);
    }

    public virtual Ticket? Get(int? id) {
        if (id is null) {
            throw new ArgumentNullException();
        }

        return _ticketRepository.Get(t => t.Id == id);
    }
    
    public virtual void Create(Ticket? ticket) {
        if (ticket is null) {
            throw new ArgumentNullException();
        }
        
        _ticketRepository.Create(ticket);
        _ticketRepository.Save();
    }

    public virtual void Update(Ticket? oldTicket, Ticket? updatedTicket, string? userId) {
        if (oldTicket is null || updatedTicket is null || userId is null) {
            throw new ArgumentNullException();
        }

        string properties = "";
        
        if (oldTicket?.Title != updatedTicket?.Title) {
            properties += $"Title ({updatedTicket?.Title}), ";
        }

        if (oldTicket?.Description != updatedTicket?.Description) {
            properties += $"Description ({updatedTicket?.Description}), ";
        }
        
        if (oldTicket?.TicketTypeId != updatedTicket?.TicketTypeId) {
            properties += $"TicketTypeId ({updatedTicket?.TicketTypeId}), ";
        }
        
        if (oldTicket?.TicketStatusId != updatedTicket?.TicketStatusId) {
            properties += $"TicketStatusId ({updatedTicket?.TicketStatusId}), ";
        }
        
        if (oldTicket?.AssignedToUserId != updatedTicket?.AssignedToUserId) {
            properties += $"AssignedToUserId ({updatedTicket?.AssignedToUserId})";
        }

        if (updatedTicket == null) return;
        updatedTicket.Updated = DateTime.Now;

        _ticketRepository.Update(updatedTicket);

        TicketLogItem ticketLogItem = new() {
            Title = oldTicket.Title,
            Description = oldTicket.Description,
            Updated = oldTicket.Updated,
            TicketType = oldTicket.TicketTypeId.ToString(),
            TicketPriority = oldTicket.TicketPriorityId.ToString(),
            TicketStatus = oldTicket.TicketStatusId.ToString(),
            AssignedToUser = oldTicket.AssignedToUserId,
        };

        _ticketHistoryRepository.Create(new() {
            TicketId = updatedTicket.Id,
            TicketLogItemId = ticketLogItem.Id,
            Properties = properties
        });
        _ticketHistoryRepository.Save();
        _ticketLogItemRepository.Create(ticketLogItem);
        _ticketRepository.Update(updatedTicket);
        _ticketRepository.Save();
    }
}
