using System.Linq.Expressions;

namespace QuizCraft.Domain.API.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<T> CreateAsync(T entity);
    IEnumerable<T> RetrieveAll();
    Task<T?> RetrieveByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
    IEnumerable<T> RetrieveByCondition(Expression<Func<T, bool>> predicate);
    void Delete(T entity);
    Task<bool> SaveChangesAsync();
    IEnumerable<TDestination> RetrieveProjected<TSource, TDestination>()
        where TSource : class
        where TDestination : class;
}