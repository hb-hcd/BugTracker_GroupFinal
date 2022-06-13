using BugTracker.Models;

namespace BugTracker.DAL; 

// ReSharper disable once InconsistentNaming
public interface IRepositoryCR<T> where T : class {
    List<T> GetList(Func<T, bool>? whereFunction);
    T? Get(Func<T, bool>? firstFunction);
    void Create(T? entity);
    void Save();
}
