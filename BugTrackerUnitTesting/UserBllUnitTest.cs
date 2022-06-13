using System.Collections.Generic;
using System.Linq;
using BugTracker.BLL;
using System;
using System.Threading;
using System.Threading.Tasks;
using BugTracker.DAL;
using BugTracker.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BugTrackerUnitTesting;

[TestClass]
public class UserBllUniTest {
    
    private UserBusinessLogicLayer _mockUserBll;
    Mock<IUserStore<User>> mockUserStore;
    Mock<IRepositoryCRUD<User>> userRepository;
    Mock<UserManager<User>> userManager;
    Mock<SignInManager<User>> signInManager;
    Mock<RoleManager<IdentityRole>> roleManager;

    [TestInitialize]
    public void Initialize() {
        Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>(MockBehavior.Default);
        Mock<IUserClaimsPrincipalFactory<User>> userClaimsPrincipalMock = new Mock<IUserClaimsPrincipalFactory<User>>();
        Mock<IOptions<IdentityOptions>> options = new Mock<IOptions<IdentityOptions>>();
        Mock<ILogger<SignInManager<User>>> logger = new Mock<ILogger<SignInManager<User>>>();
        Mock<IAuthenticationSchemeProvider> authentication = new Mock<IAuthenticationSchemeProvider>();
        Mock<IUserConfirmation<User>> userConfirmation = new Mock<IUserConfirmation<User>>();
        
         mockUserStore = new Mock<IUserStore<User>>();
         userRepository = new Mock<IRepositoryCRUD<User>>();
         userManager = new Mock<UserManager<User>>(mockUserStore.Object, options.Object, null, null, null, null, null, null, null);
         signInManager = new Mock<SignInManager<User>>(userManager.Object, httpContextAccessor.Object, userClaimsPrincipalMock.Object, options.Object, logger.Object, authentication.Object, userConfirmation.Object);
         roleManager = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object, new IRoleValidator<IdentityRole>[0], new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
        
        
        User admin = new() {
            Id = "105c4989-f8fa-4b3e-bbb6-675e138691fb",
            UserName = "admin",
            Email = "admin@bug-tracker.com",
            NormalizedEmail = "ADMIN@BUG-TRACKER.COM",
            NormalizedUserName = "ADMIN",
            EmailConfirmed = true
        };
            
        User projectManager = new() {
            Id = "1ccbe77c-6241-46e9-aacc-91a7a47560d5",
            UserName = "projectmanager",
            Email = "projectmanager@bug-tracker.com",
            NormalizedEmail = "PROJECTMANAGER@BUG-TRACKER.COM",
            NormalizedUserName = "PROJECTMANAGER",
            EmailConfirmed = true
        };
            
        User developer = new() {
            Id = "e30809ac-038e-47b7-ad75-f413a10ea553",
            UserName = "developer",
            Email = "developer@bug-tracker.com",
            NormalizedEmail = "DEVELOPER@BUG-TRACKER.COM",
            NormalizedUserName = "DEVELOPER",
            EmailConfirmed = true
        };
            
        User submitter = new() {
            Id = "d3a14aa1-2535-4fdb-8ea5-62779cd2485e",
            UserName = "submitter",
            Email = "submitter@bug-tracker.com",
            NormalizedEmail = "SUBMITTER@BUG-TRACKER.COM",
            NormalizedUserName = "SUBMITTER",
            EmailConfirmed = true
        };
            
        User guestUser = new() {
            Id = "5a0db8f0-4936-4de6-81af-ac594c5fdee4",
            UserName = "guest",
            Email = "guest@bug-tracker.com",
            NormalizedEmail = "GUEST@BUG-TRACKER.COM",
            NormalizedUserName = "GUEST",
            EmailConfirmed = true
        };
        
        var users = new List<User>() {
            admin,
            projectManager,
            developer,
            submitter,
            guestUser
        };

        userRepository.Setup(x => x.GetList(It.IsAny<Func<User, bool>>())).Returns(users);
        userRepository.Setup(x => x.Get(It.IsAny<Func<User, bool>>())).Returns(new User());
        userRepository.Setup(x => x.Create(It.IsAny<User>()));
        userRepository.Setup(x => x.Update(It.IsAny<User>()));
        userRepository.Setup(x => x.Delete(It.IsAny<User>()));
        
        
        _mockUserBll = new UserBusinessLogicLayer(userRepository.Object, userManager.Object, roleManager.Object, signInManager.Object);
    }

    [TestMethod]
    public void GetAllUsers_ShouldReturnAllUsers() {
        // Arrange
 
        
        // Act
        var result = _mockUserBll.GetAll();
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Count());
    }
    
    [TestMethod]
    public void GetUserById_ShouldReturnUser() {
        // Arrange
        var userId = "105c4989-f8fa-4b3e-bbb6-675e138691fb";
        
        // Act
        var result = _mockUserBll.GetUserById(userId);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result.Id);
    }
    
    [TestMethod]
    public void GetUserByName_ShouldReturnUser() {
        // Arrange
        var userName = "admin";
        
        // Act
        var result = _mockUserBll.GetUserByName(userName);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userName, result.UserName);
    }

    [TestMethod]
    public void UpdateUser_ShouldUpdateUser() {
        // Arrange
        var userId = "105c4989-f8fa-4b3e-bbb6-675e138691fb";
        var user = _mockUserBll.GetUserById(userId);
        user.UserName = "newUserName";
        var updatedUserName = "newUserName";
        
        // Act
        _mockUserBll.Update(user);
        user = _mockUserBll.GetUserById(userId);
        Assert.AreEqual(updatedUserName, user.UserName);
    }
    
    [TestMethod]
    public void AssignToRole_ShouldAssignUserToRole() {
        // Arrange
        var userId = "105c4989-f8fa-4b3e-bbb6-675e138691fb";
        var user = _mockUserBll.GetUserById(userId);
        var role = "Admin";
        userManager.Setup( userManager => userManager.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new User());
        userManager.Setup(u => u.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
        
        // Act
        _mockUserBll.AssignToRole(user.Id, role);
        //user = _mockUserBll.GetUserById(userId);
        Assert.IsTrue(userManager.Object.IsInRoleAsync(user, role).Result);
    }
}
