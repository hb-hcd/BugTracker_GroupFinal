namespace BugTracker.DAL; 

// ReSharper disable once InconsistentNaming
public interface IRepositoryCRU<T> : IRepositoryCR<T>, IRepositoryU<T> where T : class {
}