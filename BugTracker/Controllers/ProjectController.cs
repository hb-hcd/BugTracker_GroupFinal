using BugTracker.BLL;
using BugTracker.DAL;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracker.Controllers; 

[Authorize(Roles = "Admin, Project Manager")]
public class ProjectController : Controller {
    private readonly ProjectBusinessLogicLayer _projectBll;
    private readonly ProjectUserBusinessLogicLayer _projectUserBll;
    private readonly UserBusinessLogicLayer _userBll;

    public ProjectController(IRepositoryCRUD<Project> projectRepository, IRepositoryCRD<ProjectUser> projectUserRepository, UserManager<User> userManager, SignInManager<User> signInManager, IRepositoryCRUD<User> userRepository, RoleManager<IdentityRole> roleManager) {
        _projectUserBll = new ProjectUserBusinessLogicLayer(projectUserRepository);
        _projectBll = new ProjectBusinessLogicLayer(projectRepository);
        _userBll = new UserBusinessLogicLayer(userRepository, userManager, roleManager, signInManager);
    }

    // Get /project/create
    [HttpGet]
    public IActionResult Create() {
        return View();
    }
    
    // Post /project/create
    [HttpPost]
    public IActionResult Create([Bind("Name")] Project project) {
        try {
            var user = _userBll.GetUserByName(User.Identity?.Name);
            project.UserId = user.Id;
            _projectBll.Create(project);
            return RedirectToAction("Index", "Home");
        }
        catch (ArgumentNullException) {
            TempData["Message"] = "No value passed";
            return View("Error");
        }
    }
    
    // Get /project/update
    [HttpGet]
    public IActionResult Update(int? id) {
        if (id is null) {
            return BadRequest();
        }
        var project = _projectBll.Get(id);
        return View(project);
    }
    
    // Post /project/create
    [HttpPost]
    public IActionResult Update(int? id, string? name) {
        if (id is null) {
            return BadRequest();
        }
        try {
            var project = _projectBll.Get(id);
            project.Name = name;
            _projectBll.Edit(project);
            return RedirectToAction("Index", "Home");
        }
        catch (ArgumentNullException) {
            TempData["Message"] = "No value passed";
            return View("Error");
        }
    }
    
    // Get /project/AsssignUser
    [HttpGet]
    public IActionResult AssignUser() {
        ViewBag.projects = new SelectList(_projectBll.GetAll(), "Id", "Name");
        ViewBag.users = new SelectList(_userBll.GetAll(), "Id", "UserName");

        return View();
    }
    
    [HttpPost]
    // Post /project/AssignUser
    public IActionResult AssignUser(string? userId, int projectId) {
        var pu = _projectUserBll.Get(userId, projectId);
        if (pu is not null) {
            TempData["Message"] = "Already Assigned to project";
            return View("Error");
        }
        try {
            ProjectUser assign = new() {
                ProjectId = projectId,
                UserId = userId
            };
            
            _projectUserBll.Assign(assign);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // Get /project/UnAssignUser
    [HttpGet]
    public IActionResult UnAssignUser() {
        ViewBag.projects = new SelectList(_projectBll.GetAll(), "Id", "Name");
        ViewBag.users = new SelectList(_userBll.GetAll(), "Id", "UserName");

        return View();
    }
    
    // Post /project/UnAssignUser
    [HttpPost]
    public IActionResult UnAssignUser(string? userId, int projectId) {
        var pu = _projectUserBll.Get(userId, projectId);
        if (pu is null) {
            TempData["Message"] = "user not Assigned to project";
            return View("Error");
        }
        try {
            _projectUserBll.UnAssign(new(){UserId = userId, ProjectId = projectId});
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}
