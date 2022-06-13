namespace BugTracker.DAL; 

// ReSharper disable once InconsistentNaming
public interface IRepositoryCRD<T> : IRepositoryCR<T>, IRepositoryD<T> where T : class {
}