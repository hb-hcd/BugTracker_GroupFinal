using System.ComponentModel.DataAnnotations;
using BugTracker.BLL;
using BugTracker.DAL;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracker.Controllers; 

[Authorize]
public class UserController : Controller {
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserBusinessLogicLayer _userBll;

    public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IRepositoryCRUD<User> userRepository, RoleManager<IdentityRole> roleManager) {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userBll = new UserBusinessLogicLayer(userRepository, userManager, roleManager, signInManager);
    }

    // GET /users/editProfile
    [HttpGet]
    public IActionResult EditProfile() {
        var user = _userBll.GetUserByName(User.Identity.Name);
        return View(user);
    }
    
    // Put /users/editProfile
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(string? id, string? email, string? username) {
        if (id is null) {
            return BadRequest();
        }
        
        var user = _userBll.GetUserById(id);
        
        if (email is not null) {
            user.Email = email;
            user.NormalizedEmail = email.ToUpper();
        }

        if (username is not null) {
            user.UserName = username;
            user.NormalizedUserName = username.ToUpper();
        }

        _userBll.Update(user);

        await _signInManager.RefreshSignInAsync(user);
        
        return RedirectToAction("Index", "Home");
    }
    
    [Authorize(Roles = "Admin")]
    // GET /user/assignRole
    [HttpGet]
    public IActionResult AssignRole() {
        ViewBag.users = new SelectList(_userBll.GetAll(), "Id", "UserName");
        ViewBag.roles = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    // Post /user/assignRole
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignRole(string? userId, string? roleId) {
        if (userId is null || roleId is null) {
            return BadRequest();
        }
        
        try {
            User user = _userBll.GetUserById(userId);

            await _userBll.AssignToRole(userId, roleId);
        
            return RedirectToAction("Index", "Home");
        }
        catch (ArgumentNullException e) {
            TempData["Message"] = "User no found";
            return View("Error");
        }
    }
    
    // GET /user/updatePassword
    [HttpGet]
    public IActionResult UpdatePassword() {
        return View();
    }

    public class UpdatePasswordInput {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    
    // Post /user/updatePassword
    [HttpPost]
    public async Task<IActionResult> UpdatePassword(string? oldPassword, string? newPassword, string? confirmPassword) {
        var user = _userBll.GetUserByName(User.Identity.Name);

        if ((await _userBll.CheckPassword(oldPassword, user.Id)) == false) {
            TempData["Message"] = "Old password did not match";
            return View("Error");
        }
        if (newPassword != confirmPassword) {
            TempData["Message"] = "password does match";
            return View("Error");
        }
        var passwordHasher = new PasswordHasher<User>();
        var newPasswordHash = passwordHasher.HashPassword(user, newPassword);
        user.PasswordHash = newPasswordHash;
        
        _userBll.Update(user);
        
        return RedirectToAction("Index", "Home");
    }
}