using System;
using System.Collections.Generic;
using BugTracker.BLL;
using BugTracker.DAL;
using BugTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BugTrackerUnitTesting;

[TestClass]
public class TicketBllUniTest {
    private Mock<TicketBusinessLogicLayer> _ticketBll;
    private Mock<IRepositoryCRU<Ticket>> _ticketRepo;
    private Mock<IRepositoryCR<TicketHistory>> _ticketHistoryRepo;
    private Mock<IRepositoryCR<TicketLogItem>> _ticketLogRepo;

    [TestInitialize]
    public void Initialize() {
        _ticketRepo = new Mock<IRepositoryCRU<Ticket>>();
        _ticketHistoryRepo = new Mock<IRepositoryCR<TicketHistory>>();
        _ticketLogRepo = new Mock<IRepositoryCR<TicketLogItem>>();
        _ticketBll = new Mock<TicketBusinessLogicLayer>(_ticketRepo.Object, _ticketLogRepo.Object, _ticketHistoryRepo.Object);
        
        Ticket ticket = new Ticket {
            Id = 1,
            Title = "Test Ticket",
            Description = "Test Description",
            Created = DateTime.Now,
            Updated = DateTime.Now,
            ProjectId = 1,
            TicketTypeId = 1,
            TicketPriorityId = 1,
            TicketStatusId = 1,
            OwnerUserId = "1",
            AssignedToUserId = "2",
        };
        
        Ticket ticket2 = new Ticket {
            Id = 2,
            Title = "Test Ticket 2",
            Description = "Test Description 2",
            Created = DateTime.Now,
            Updated = DateTime.Now,
            ProjectId = 2,
            TicketTypeId = 2,
            TicketPriorityId = 2,
            TicketStatusId = 2,
            OwnerUserId = "1",
            AssignedToUserId = "2",
        };
        
        List<Ticket> tickets = new List<Ticket> {
            ticket,
            ticket2
        };
        
        _ticketBll.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(tickets);
        _ticketBll.Setup(x => x.Get(It.IsAny<int>())).Returns(ticket);
        _ticketBll.Setup(x => x.Create(It.IsAny<Ticket>())).Callback((Ticket t) => tickets.Add(t));
        _ticketBll.Setup(x => x.Update(It.IsAny<Ticket?>(), It.IsAny<Ticket?>(), It.IsAny<string?>())).Callback((Ticket t, Ticket ut, string uid) => tickets[tickets.IndexOf(ticket)] = t);
    }
    
    
    [TestMethod]
    public void GetAllTest() {
        List<Ticket> tickets = _ticketBll.Object.GetAll(1, 1);
        Assert.AreEqual(2, tickets.Count);
    }
    
    [TestMethod]
    public void GetTest() {
        Ticket ticket = _ticketBll.Object.Get(1);
        Assert.AreEqual(1, ticket.Id);
    }
    
    [TestMethod]
    public void CreateTest() {
        Ticket ticket = new Ticket {
            Id = 3,
            Title = "Test Ticket 3",
            Description = "Test Description 3",
            Created = DateTime.Now,
            Updated = DateTime.Now,
            ProjectId = 3,
            TicketTypeId = 3,
            TicketPriorityId = 3,
            TicketStatusId = 3,
            OwnerUserId = "1",
            AssignedToUserId = "2",
        };
        _ticketBll.Object.Create(ticket);
        Assert.AreEqual(3, _ticketBll.Object.GetAll(1, 1).Count);
    }
    
    [TestMethod]
    public void UpdateTest() {
        Ticket ticket = _ticketBll.Object.Get(1);
        ticket.Title = "Test Ticket Updated";
        Ticket updated = ticket;
        _ticketBll.Object.Update(ticket, updated, null);
        Assert.AreEqual("Test Ticket Updated", _ticketBll.Object.Get(1).Title);
    }
}
