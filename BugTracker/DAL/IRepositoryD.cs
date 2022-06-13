namespace BugTracker.DAL; 

public interface IRepositoryD<T> where T : class {
    void Delete(T? entity);
}
