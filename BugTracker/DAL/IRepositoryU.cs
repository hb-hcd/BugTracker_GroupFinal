namespace BugTracker.DAL; 

public interface IRepositoryU<T> where T : class {
    void Update(T? entity);
}
