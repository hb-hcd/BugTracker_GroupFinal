namespace BugTracker.DAL; 

// ReSharper disable once InconsistentNaming
public interface IRepositoryCRUD<T> : IRepositoryCR<T>, IRepositoryU<T>, IRepositoryD<T> where T : class {
}