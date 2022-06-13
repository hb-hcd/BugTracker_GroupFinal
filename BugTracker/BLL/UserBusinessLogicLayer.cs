using BugTracker.DAL;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.BLL;

public class UserBusinessLogicLayer {
    private readonly IRepositoryCRUD<User> _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;

    public UserBusinessLogicLayer(IRepositoryCRUD<User> userRepository, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) {
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    public List<User> GetAll() {
        return _userRepository.GetList(_ => true);
    }
    
    public void Update(User? user) {
        if (user is null) {
            throw new ArgumentNullException(nameof(user));
        }
        
        _userRepository.Update(user);
        _userRepository.Save();
    }

    public async Task AssignToRole(string? userId, string? roleId) {
        if(userId is null) {
            throw new ArgumentNullException(nameof(userId));
        }
        if(roleId is null) {
            throw new ArgumentNullException(nameof(roleId));
        }
        var user = GetUserById(userId);
        if(user is null) {
            throw new ArgumentNullException(nameof(user));
        }
        IdentityRole role = await _roleManager.FindByIdAsync(roleId);
        
        if(role is null) {
            throw new ArgumentException(nameof(role));
        }
        
        await _userManager.AddToRoleAsync(user, role.Name);
        _userRepository.Save();
    }
    
    public async Task<bool> CheckPassword(string? password, string? userId) {
        if(password is null) {
            throw new ArgumentNullException(nameof(password));
        }
        if(userId is null) {
            throw new ArgumentNullException(nameof(userId));
        }
        var user = GetUserById(userId);
        if(user is null) {
            throw new ArgumentException(nameof(user));
        }
        return (await _signInManager.CheckPasswordSignInAsync(user, password, false)).Succeeded;
    }

    public User GetUserByName(string? name) {
        if (name is null) {
            throw new ArgumentNullException();
        }
        
        var user = _userRepository.GetList(u => u.UserName == name).FirstOrDefault();
        if (user is null) {
            throw new ArgumentException();
        }
        return user;
    }
    
    public User GetUserById(string? userId) {
        if (userId is null) {
            throw new ArgumentNullException();
        }
        
        var user = _userRepository.GetList(u => u.Id == userId).FirstOrDefault();
        if (user is null) {
            throw new ArgumentException();
        }
        return user;
    }
}
