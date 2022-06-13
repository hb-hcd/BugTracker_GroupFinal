using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.DAL;

public class UserRepository : IRepositoryCRUD<User> {

    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) {
        _context = context;
    }

    public List<User> GetList(Func<User, bool>? whereFunction) {
        if (whereFunction is null) {
            throw new ArgumentNullException();
        }
        
        return _context.Users.Where(whereFunction).ToList();
    }

    public User Get(Func<User, bool>? firstFunction) {
        if (firstFunction is null) {
            throw new ArgumentNullException();
        }
        return _context.Users.First(firstFunction);
    }

    public void Create(User? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.Users.Add(entity);
    }

    public void Save() {
        _context.SaveChanges();
    }

    public void Update(User? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.Users.Update(entity);
    }

    public void Delete(User? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.Users.Remove(entity);
    }
}
