using System;
using System.Collections.Generic;
using BugTracker.BLL;
using BugTracker.DAL;
using BugTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BugTrackerUnitTesting;

[TestClass]
public class TicketAttachmentBllUnitTest {
    private Mock<TicketAttachmentBll> mockTicketAttachmentBll;
    
    private Mock<IRepositoryCRUD<TicketAttachment>> mockTicketAttachmentRepository;

    private List<TicketAttachment> ticketAttachments;
    
    [TestInitialize]
    public void TestInitialize() {
        mockTicketAttachmentRepository = new Mock<IRepositoryCRUD<TicketAttachment>>();
        mockTicketAttachmentBll = new Mock<TicketAttachmentBll>(mockTicketAttachmentRepository.Object);
        
        TicketAttachment ticketAttachment1 = new TicketAttachment {
            Id = 1,
            TicketId = 1,
            FilePath = "FilePath1",
            Description = "Description1",
            Created = DateTime.Now,
            UserId = "1"
        };
        
        TicketAttachment ticketAttachment2 = new TicketAttachment {
            Id = 2,
            TicketId = 2,
            FilePath = "FilePath2",
            Description = "Description2",
            Created = DateTime.Now,
            UserId = "2"
        };
        
        TicketAttachment ticketAttachment3 = new TicketAttachment {
            Id = 3,
            TicketId = 3,
            FilePath = "FilePath3",
            Description = "Description3",
            Created = DateTime.Now,
            UserId = "3"
        };
         
        ticketAttachments = new List<TicketAttachment> {
            ticketAttachment1,
            ticketAttachment2,
            ticketAttachment3
        };
        
        mockTicketAttachmentBll.Setup(x => x.GetTicketAttachments(It.IsAny<int>())).Returns(ticketAttachments); 
        
        mockTicketAttachmentBll.Setup(x => x.GetTicketAttachment(It.IsAny<int>())).Returns(ticketAttachment1);
        
        mockTicketAttachmentBll.Setup(x => x.CreateAttachment(It.IsAny<TicketAttachment?>())).Callback((TicketAttachment? entity) => {
            ticketAttachments.Add(entity);
        });
        
        mockTicketAttachmentBll.Setup(x => x.UpdateAttachment(It.IsAny<TicketAttachment?>())).Callback((TicketAttachment? entity) => {
            var index = ticketAttachments.FindIndex(x => x.Id == entity.Id);
            ticketAttachments[index] = entity;
        });
        
        mockTicketAttachmentBll.Setup(x => x.DeleteAttachment(It.IsAny<TicketAttachment?>())).Callback((TicketAttachment? entity) => {
            var index = ticketAttachments.FindIndex(x => x.Id == entity.Id);
            ticketAttachments.RemoveAt(index);
        });
    }
    
    [TestMethod]
    public void GetTicketAttachments_ShouldReturnListOfTicketAttachments() {
        // Arrange
        int ticketId = 1;
        
        // Act
        var ticketAttachments = mockTicketAttachmentBll.Object.GetTicketAttachments(ticketId);
        
        // Assert
        Assert.IsNotNull(ticketAttachments);
        Assert.IsInstanceOfType(ticketAttachments, typeof(List<TicketAttachment>));
        Assert.AreEqual(3, ticketAttachments.Count);
    }
    
    [TestMethod]
    public void GetTicketAttachment_ShouldReturnTicketAttachment() {
        // Arrange
        int ticketAttachmentId = 1;
        
        // Act
        var ticketAttachment = mockTicketAttachmentBll.Object.GetTicketAttachment(ticketAttachmentId);
        
        // Assert
        Assert.IsNotNull(ticketAttachment);
        Assert.IsInstanceOfType(ticketAttachment, typeof(TicketAttachment));
        Assert.AreEqual(1, ticketAttachment.Id);
    }
    
    [TestMethod]
    public void CreateAttachment_ShouldAddTicketAttachmentToList() {
        // Arrange
        TicketAttachment ticketAttachment = new TicketAttachment {
            TicketId = 1,
            FilePath = "FilePath1",
            Description = "Description1",
            Created = DateTime.Now,
            UserId = "1"
        };
        
        // Act
        mockTicketAttachmentBll.Object.CreateAttachment(ticketAttachment);
        
        // Assert
        Assert.AreEqual(4, ticketAttachments.Count);
    }
    
    
    [TestMethod]
    public void UpdateAttachment_ShouldUpdateTicketAttachmentInList() {
        // Arrange
        TicketAttachment ticketAttachment = ticketAttachments.Find(ta => ta.Id == 1);
        
        ticketAttachment.FilePath = "FilePath1Updated";
        
        // Act
        mockTicketAttachmentBll.Object.UpdateAttachment(ticketAttachment);

        var ta = mockTicketAttachmentBll.Object.GetTicketAttachment(ticketAttachment.Id);
        
        // Assert
        Assert.AreEqual(ta, ticketAttachment);
    }   
    
    [TestMethod]
    public void DeleteAttachment_ShouldRemoveTicketAttachmentFromList() {
        // Arrange
        int ticketAttachmentId = 1;
        var tA = ticketAttachments.Find(ta => ta.Id == ticketAttachmentId);
        
        // Act
        mockTicketAttachmentBll.Object.DeleteAttachment(tA);
        
        // Assert
        Assert.AreEqual(2, ticketAttachments.Count);
    }
}
