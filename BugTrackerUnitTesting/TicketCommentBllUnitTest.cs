using System;
using System.Collections.Generic;
using BugTracker.BLL;
using BugTracker.DAL;
using BugTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BugTrackerUnitTesting;

[TestClass]
public class TicketCommentBllUnitTest {
    private Mock<TicketCommentBll> mockTicketCommentBll;
    
    private Mock<IRepositoryCRUD<TicketComment>> mockTicketCommentRepository;

    private List<TicketComment> ticketComments;
    
    [TestInitialize]
    public void TestInitialize() {
        mockTicketCommentRepository = new Mock<IRepositoryCRUD<TicketComment>>();
        mockTicketCommentBll = new Mock<TicketCommentBll>(mockTicketCommentRepository.Object);
        
        TicketComment ticketComment1 = new TicketComment {
            Id = 1,
            Comment = "This is a test comment",
            Created = DateTime.Now,
        };
        
        TicketComment ticketComment2 = new TicketComment {
            Id = 2,
            Comment = "This is a test comment",
            Created = DateTime.Now,
        };
        
        TicketComment ticketComment3 = new TicketComment {
            Id = 3,
            Comment = "This is a test comment",
            Created = DateTime.Now,
        };
         
        ticketComments = new List<TicketComment> {
            ticketComment1,
            ticketComment2,
            ticketComment3
        };
        
        mockTicketCommentBll.Setup(x => x.GetTicketCommentsByTicketId(It.IsAny<int>())).Returns(ticketComments); 
        
         mockTicketCommentBll.Setup(x => x.GetCommentById(It.IsAny<int>())).Returns(ticketComment1);
        
        mockTicketCommentBll.Setup(x => x.CreateComment(It.IsAny<TicketComment?>())).Callback((TicketComment? entity) => {
            ticketComments.Add(entity);
        });
        
        mockTicketCommentBll.Setup(x => x.UpdateComment(It.IsAny<TicketComment?>())).Callback((TicketComment? entity) => {
            var index = ticketComments.FindIndex(x => x.Id == entity.Id);
            ticketComments[index] = entity;
        });
        
        mockTicketCommentBll.Setup(x => x.DeleteComment(It.IsAny<TicketComment?>())).Callback((TicketComment? entity) => {
            var index = ticketComments.FindIndex(x => x.Id == entity.Id);
            ticketComments.RemoveAt(index);
        });
    }
    
    [TestMethod]
    public void GetTicketComments_ShouldReturnListOfTicketComments() {
        // Arrange
        int ticketId = 1;
        
        // Act
        var ticketComment = mockTicketCommentBll.Object.Get(ticketId);
        
        // Assert
        Assert.IsNotNull(ticketComments);
        Assert.IsInstanceOfType(ticketComments, typeof(List<TicketComment>));
        Assert.AreEqual(3, ticketComments.Count);
    }
    
    [TestMethod]
    public void GetTicketComment_ShouldReturnTicketComment() {
        // Arrange
        int ticketCommentId = 1;
        
        // Act
        var ticketComment = mockTicketCommentBll.Object.GetCommentById(ticketCommentId);
        
        // Assert
        Assert.IsNotNull(ticketComment);
        Assert.IsInstanceOfType(ticketComment, typeof(TicketComment));
        Assert.AreEqual(1, ticketComment.Id);
    }
    
    [TestMethod]
    public void CreateComment_ShouldAddTicketCommentToList() {
        // Arrange
        TicketComment ticketComment = new TicketComment {
            Id = 4,
            Comment = "This is a test comment",
            Created = DateTime.Now,
        };
        
        // Act
        mockTicketCommentBll.Object.CreateComment(ticketComment);
        
        // Assert
        Assert.AreEqual(4, ticketComments.Count);
    }
    
    
    [TestMethod]
    public void UpdateComment_ShouldUpdateTicketCommentInList() {
        // Arrange
        TicketComment ticketComment = ticketComments.Find(ta => ta.Id == 1);
        
        ticketComment.Comment = "This is a test comment updated";
        
        // Act
        mockTicketCommentBll.Object.UpdateComment(ticketComment);

        var ta = mockTicketCommentBll.Object.GetCommentById(ticketComment.Id);
        
        // Assert
        Assert.AreEqual(ta, ticketComment);
    }   
    
    [TestMethod]
    public void DeleteComment_ShouldRemoveTicketCommentFromList() {
        // Arrange
        int ticketCommentId = 1;
        var tA = ticketComments.Find(ta => ta.Id == ticketCommentId);
        
        // Act
        mockTicketCommentBll.Object.DeleteComment(tA);
        
        // Assert
        Assert.AreEqual(2, ticketComments.Count);
    }
}
