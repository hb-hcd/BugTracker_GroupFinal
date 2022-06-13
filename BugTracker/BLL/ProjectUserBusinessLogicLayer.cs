using BugTracker.DAL;
using BugTracker.Models;

namespace BugTracker.BLL; 

public class ProjectUserBusinessLogicLayer {
    private readonly IRepositoryCRD<ProjectUser> _projectUserRepository;
    public ProjectUserBusinessLogicLayer(IRepositoryCRD<ProjectUser> projectUserRepository) {
        _projectUserRepository = projectUserRepository;
    }

    public  virtual List<ProjectUser> GetByUserId(string? userId) {
        if (userId is null) {
            throw new ArgumentNullException();
        }

        return _projectUserRepository.GetList(pu => pu.UserId == userId);
    }
    
    public virtual ProjectUser? Get(string? userId, int? projectId) {
        if (userId is null || projectId is null) {
            throw new ArgumentNullException();
        }

        return _projectUserRepository.Get(pu => pu.UserId == userId && pu.ProjectId == projectId);
    }

    public virtual List<Project?> GetAssignedProject(string? userId) {
        return _projectUserRepository.GetList(pu => pu.UserId == userId).Select(pu => pu.Project).ToList();
    }
    
    public virtual void Assign(ProjectUser? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }
        _projectUserRepository.Create(entity);
        _projectUserRepository.Save();
    }
    
    public virtual void UnAssign(ProjectUser? entity) {
        if (entity is null) {
            throw new ArgumentNullException();
        }
        
        _projectUserRepository.Delete(entity);
        _projectUserRepository.Save();
    }
}
