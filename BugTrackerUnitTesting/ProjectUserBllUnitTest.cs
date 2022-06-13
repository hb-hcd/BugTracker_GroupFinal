using System;
using System.Collections.Generic;
using BugTracker.BLL;
using BugTracker.DAL;
using BugTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BugTrackerUnitTesting;

[TestClass]
public class ProjectUserBllUnitTest {
    private Mock<ProjectUserBusinessLogicLayer> _mockProjectUserBll;
    private Mock<IRepositoryCRD<ProjectUser>> _mockProjectUserRepo;

    private List<ProjectUser> projectUsers;
    
    [TestInitialize]
    public void Initialize() {
        _mockProjectUserRepo = new Mock<IRepositoryCRD<ProjectUser>>();
        _mockProjectUserBll = new Mock<ProjectUserBusinessLogicLayer>(_mockProjectUserRepo.Object);
        
        Project project1 = new() {
            Id = 1,
            Name = "First Project",
            UserId = "1"
        };
        Project project2 = new() {
            Id = 2,
            Name = "Second Project",
            UserId = "2"
        };
        Project project3 = new() {
            Id = 3,
            Name = "Third Project",
            UserId = "3"
        };
        List<Project> projects = new() {
            project1,
            project2,
            project3,
        };
        
        ProjectUser projectUser1 = new ProjectUser {
            ProjectId = 1,
            UserId = "1",
        };
        
        ProjectUser projectUser2 = new ProjectUser {
            ProjectId = 2,
            UserId = "2",
        };
        
        ProjectUser projectUser3 = new ProjectUser {
            ProjectId = 3,
            UserId = "3",
        };
        
        
        projectUsers = new List<ProjectUser> {
            projectUser1,
            projectUser2,
            projectUser3,
        };
        
        _mockProjectUserBll.Setup(x => x.GetByUserId(It.IsAny<string?>())).Returns((string? userId) => {
            return projectUsers.FindAll(x => x.UserId == userId);
        });
        
        _mockProjectUserBll.Setup(x => x.Get(It.IsAny<string?>(), It.IsAny<int?>())).Returns((string? userId, int? projectId) => {
            return projectUsers.Find(x => x.ProjectId == projectId && x.UserId == userId);
        });
        
        _mockProjectUserBll.Setup(x => x.GetAssignedProject(It.IsAny<string?>())).Returns((string? userId) => {
            return projects.FindAll(x => x.UserId == userId);
        });
        
        _mockProjectUserBll.Setup(x => x.Assign(It.IsAny<ProjectUser?>())).Callback((ProjectUser? projectUser) => {
            projectUsers.Add(projectUser);
        });
        
        _mockProjectUserBll.Setup(x => x.UnAssign(It.IsAny<ProjectUser?>())).Callback((ProjectUser? entity) => {
            projectUsers.RemoveAll(x => x.ProjectId == entity.Id && x.UserId == entity.UserId);
        });
        
    }
    
    [TestMethod]
    public void GetByUserIdTest() {
        string userId = "1";
        List<ProjectUser> projectUser = _mockProjectUserBll.Object.GetByUserId(userId);
        
        
        Assert.IsNotNull(projectUser);
    }
    
    [TestMethod]
    public void GetTest() {
        string userId = "1";
        int projectId = 1;
        ProjectUser projectUser = _mockProjectUserBll.Object.Get(userId, projectId);
        
        Assert.IsNotNull(projectUser);
    }
    
    [TestMethod]
    public void GetAssignedProjectTest() {
        string userId = "1";
        List<Project?> projectUser = _mockProjectUserBll.Object.GetAssignedProject(userId);
        
        Assert.IsNotNull(projectUser);
    }
    
    
    [TestMethod]
    public void AssignTest() {
        ProjectUser projectUser = new ProjectUser {
            ProjectId = 1,
            UserId = "1",
        };
        
        _mockProjectUserBll.Object.Assign(projectUser);
        
        Assert.IsTrue(projectUsers.Contains(projectUser));
    }
    
    [TestMethod]    
    public void UnAssignTest() {
        ProjectUser projectUser = new ProjectUser {
            ProjectId = 1,
            UserId = "1",
        };
        
        _mockProjectUserBll.Object.UnAssign(projectUser);
        
        Assert.IsFalse(projectUsers.Contains(projectUser));
    }
}
