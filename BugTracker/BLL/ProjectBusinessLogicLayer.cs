using BugTracker.DAL;
using BugTracker.Models;

namespace BugTracker.BLL; 

public class ProjectBusinessLogicLayer {
    private readonly IRepositoryCRUD<Project> _projectRepository;
    public ProjectBusinessLogicLayer(IRepositoryCRUD<Project> projectRepository) {
        _projectRepository = projectRepository;
    }
    
    public virtual void Create(Project? project) {
        if (project is null) {
            throw new ArgumentNullException();
        }
        
        _projectRepository.Create(project);
        _projectRepository.Save();
    }

    public virtual void Edit(Project? project) {
        if (project is null) {
            throw new ArgumentNullException();
        }
        
        _projectRepository.Update(project);
        _projectRepository.Save();
    }

    public virtual List<Project> GetAll() {

        return _projectRepository.GetList(_ => true);
    }

    public virtual List<Project> GetProject(string? userId) {
        if(userId is null) {
            throw new ArgumentNullException();
        }
        
        return _projectRepository.GetList(project => project.UserId == userId);
    }

    public virtual Project? Get(int? id) {
        if (id is null) {
            throw new ArgumentNullException();
        }

        return _projectRepository.Get(p => p.Id == id);
    }
}
