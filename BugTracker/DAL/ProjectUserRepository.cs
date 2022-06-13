using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.DAL;

public class ProjectUserRepository : IRepositoryCRD<ProjectUser> {

    private readonly ApplicationDbContext _context;

    public ProjectUserRepository(ApplicationDbContext context) {
        _context = context;
    }

    public List<ProjectUser> GetList(Func<ProjectUser, bool>? whereFunction) {
        if (whereFunction is null) {
            throw new ArgumentNullException();
        }
        
        return _context.ProjectUsers
            .Include(pu => pu.User)
            .Include(pu => pu.Project)
            .Where(whereFunction).ToList();
    }

    public ProjectUser? Get(Func<ProjectUser, bool>? firstFunction) {
        if (firstFunction is null) {
            throw new ArgumentNullException();
        }
        return _context.ProjectUsers.FirstOrDefault(firstFunction);
    }

    public void Create(ProjectUser? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.ProjectUsers.Add(entity);
    }

    public void Save() {
        _context.SaveChanges();
    }

    public void Delete(ProjectUser? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }

        _context.ProjectUsers.Remove(entity);
    }
}
