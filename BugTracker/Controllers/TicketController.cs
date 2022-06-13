using BugTracker.BLL;
using BugTracker.DAL;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracker.Controllers; 

[Authorize]
public class TicketController : Controller {
    
    private readonly TicketBusinessLogicLayer _ticketBll;
    private readonly ProjectBusinessLogicLayer _projectBll;
    private readonly UserBusinessLogicLayer _userBll;
    private readonly ProjectUserBusinessLogicLayer _projectUserBll;
    
    public TicketController(
        IRepositoryCRUD<User> userRepository,
        IRepositoryCRUD<Project> projectRepository,
        IRepositoryCRD<ProjectUser> projectUserRepository,
        IRepositoryCRU<Ticket> ticketRepository,
        IRepositoryCR<TicketHistory> ticketHistoryRepository,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<User> signInManager,
        IRepositoryCR<TicketLogItem> ticketLogItemRepository)
    {
        _projectUserBll = new ProjectUserBusinessLogicLayer(projectUserRepository);
        _ticketBll = new TicketBusinessLogicLayer(ticketRepository, ticketLogItemRepository, ticketHistoryRepository);
        _userBll = new UserBusinessLogicLayer(userRepository, userManager, roleManager, signInManager);
        _projectBll = new ProjectBusinessLogicLayer(projectRepository);
    }
    
    [HttpGet]
    [Authorize(Roles = "Submitter")]
    public IActionResult Create() {
        var user = _userBll.GetUserByName(User.Identity?.Name);
        //only getting projects assigned to the submitter
        var assignedProjects = _projectUserBll.GetAssignedProject(user.Id);
        var types = from TicketTypeName t in Enum.GetValues(typeof(TicketTypeName))
                    select new {
                        Id = (int)t,
                        Name = t.ToString()
                    };

        var priorities = from TicketPriorityName t in Enum.GetValues(typeof(TicketPriorityName))
                         select new {
                             Id = (int)t,
                             Name = t.ToString()
                         };


        ViewBag.ticketTypes = new SelectList(types.ToList(), "Id", "Name");
        ViewBag.ticketPriorities = new SelectList(priorities.ToList(), "Id", "Name");
        ViewBag.projects = new SelectList(assignedProjects.ToList(), "Id", "Name");

        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Submitter")]
    public IActionResult Create([Bind("Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId")] Ticket ticket) {
        var user = _userBll.GetUserByName(User.Identity?.Name);
        ticket.OwnerUserId = user?.Id;
        ticket.Created = DateTime.Now;
        ticket.TicketStatusId = (int)TicketStatusName.Unassigned;

        try {
            _ticketBll.Create(ticket);
            return RedirectToAction(nameof(Details), new
            {
                id = ticket.Id
            });
        } catch ( Exception e ) {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet]
    public IActionResult Details(int? id) {
        if ( id is null ) {
            return BadRequest();
        }

        try {
            var ticket = _ticketBll.Get(id);

            return View(ticket);
        }
        catch (Exception e) {
            TempData["Message"] = "Ticket Not Found";
            return View("Error");
        }
    }
    
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if ( id is null )
        {
            return BadRequest();
        }

        Ticket ticket;

        try
        {
            ticket = _ticketBll.Get(id);
        }
        catch ( Exception e )
        {
            Console.WriteLine(e);
            throw;
        }

        var types = from TicketTypeName t in Enum.GetValues(typeof(TicketTypeName))
            select new
            {
                Id = (int)t,
                Name = t.ToString()
            };

        var priorities = from TicketPriorityName t in Enum.GetValues(typeof(TicketPriorityName))
            select new
            {
                Id = (int)t,
                Name = t.ToString()
            };

        ViewBag.ticketTypes = new SelectList(types.ToList(), "Id", "Name");
        ViewBag.ticketPriority = new SelectList(priorities.ToList(), "Id", "Name");

        return View(ticket);
    }

    [HttpPost]
    public IActionResult Edit(int? id, string? title, string? description, int ticketTypeId, int ticketPriorityId)
    {
        if ( id is null )
        {
            return BadRequest();
        }

        try
        {
            var ticket = _ticketBll.Get(id);

            var updatedTicket = ticket.Copy();

            updatedTicket.Title = title;
            updatedTicket.Description = description;
            updatedTicket.TicketTypeId = ticketTypeId;
            updatedTicket.TicketPriorityId = ticketPriorityId;

            var user = _userBll.GetUserByName(User.Identity?.Name);

            _ticketBll.Update(ticket, updatedTicket, user?.Id);

            return RedirectToAction(nameof(Details), new
            {
                id = ticket.Id
            });
        }
        catch ( Exception e )
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
