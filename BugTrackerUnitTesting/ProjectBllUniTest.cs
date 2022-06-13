using System.Collections.Generic;
using BugTracker.BLL;
using BugTracker.DAL;
using BugTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BugTrackerUnitTesting;

[TestClass]
public class ProjectBllUniTest {
#pragma warning disable CS8618
    private Mock<ProjectBusinessLogicLayer> _mockProjectBll;
    private Mock<IRepositoryCRUD<Project>> _mockProjectRepo;
#pragma warning restore CS8618
    
    [TestInitialize]
    public void Initialize() {
        Project project1 = new() {
            Id = 1,
            Name = "First Project",
            UserId = "1ccbe77c-6241-46e9-aacc-91a7a47560d5"
        };
        Project project2 = new() {
            Id = 2,
            Name = "Second Project",
            UserId = "1ccbe77c-6241-46e9-aacc-91a7a47560d5"
        };
        Project project3 = new() {
            Id = 3,
            Name = "Third Project",
            UserId = "e30809ac-038e-47b7-ad75-f413a10ea553"
        };
        List<Project> projects = new() {
            project1,
            project2,
            project3,
        };
        
        _mockProjectRepo = new Mock<IRepositoryCRUD<Project>>();
        
        _mockProjectBll = new Mock<ProjectBusinessLogicLayer>(_mockProjectRepo.Object);
        
        _mockProjectBll.Setup(p => p.GetAll()).Returns(projects);
        
        _mockProjectBll.Setup(p => p.GetProject(It.IsAny<string?>())).Returns((string? userId) => projects.FindAll(project => project.UserId == userId));
        
        _mockProjectBll.Setup(p => p.Get(It.IsAny<int?>())).Returns((int id) => projects.Find(p => p.Id == id));
        
        _mockProjectBll.Setup(p => p.Create(It.IsAny<Project?>())).Callback((Project? p) => {
            projects.Add(p);
        });
        
        _mockProjectBll.Setup(p => p.Edit(It.IsAny<Project?>())).Callback((Project? p) => {
            var index = projects.FindIndex(x => x.Id == p.Id);
            projects[index] = p;
        });
        
    }

    [TestMethod]
    public void GetAllProjects_ShouldReturnAllProjects() {
        // Arrange
        // Act
        var result = _mockProjectBll.Object.GetAll();
        // Assert
        Assert.AreEqual(3, result.Count);
    }

    [TestMethod]
    public void CreateProject_ShouldCreateProject() {
        // Arrange
        Project project = new() {
            Name = "Fourth Project",
        };
        // Act
        _mockProjectBll.Object.Create(project);
        // Assert
        Assert.AreEqual(4, _mockProjectBll.Object.GetAll().Count);
    }
    
    [TestMethod]
    public void UpdateProject_ShouldUpdateProject() {
        // Arrange
        Project project = _mockProjectBll.Object.Get(1);
        project.Name = "Updated Project";
        var updatedProject = "Updated Project";
        // Act
        _mockProjectBll.Object.Edit(project);
        
        // Assert
        Assert.AreEqual(updatedProject, _mockProjectBll.Object.Get(1).Name);
    }

    [TestMethod]
    public void GetProject_ShouldReturnsUserCreatedProject() {
        // Arrange
        // Act
        var result = _mockProjectBll.Object.GetProject("1ccbe77c-6241-46e9-aacc-91a7a47560d5");
        // Assert
        Assert.AreEqual(2, result.Count);
    }
}
