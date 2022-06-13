using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.DAL;

public class ProjectRepository : IRepositoryCRUD<Project> {

    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context) {
        _context = context;
    }

    public List<Project> GetList(Func<Project, bool>? whereFunction) {
        if (whereFunction is null) {
            throw new ArgumentNullException();
        }
        
        return _context.Projects.Where(whereFunction).ToList();
    }

    public Project Get(Func<Project, bool>? firstFunction) {
        if (firstFunction is null) {
            throw new ArgumentNullException();
        }
        return _context.Projects.First(firstFunction);
    }

    public void Create(Project? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.Projects.Add(entity);
    }

    public void Save() {
        _context.SaveChanges();
    }

    public void Update(Project? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.Projects.Update(entity);
    }

    public void Delete(Project? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.Projects.Remove(entity);
    }
}
