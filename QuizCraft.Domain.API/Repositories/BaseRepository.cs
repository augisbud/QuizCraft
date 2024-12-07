using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Entities;
using System.Linq.Expressions;

namespace QuizCraft.Domain.API.Repositories;

public class BaseRepository<T> where T : class, IEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly IMapper _mapper;

    public BaseRepository(DbContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _mapper = mapper;
    }

    public async Task<T> CreateAsync(T entity)
    {
        var result = await _dbSet.AddAsync(entity);
        
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<IEnumerable<T>> RetrieveAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> RetrieveByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    public IEnumerable<T> RetrieveByCondition(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.AsNoTracking().Where(predicate);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public IEnumerable<TDestination> RetrieveProjected<TSource, TDestination>()
        where TSource : class
        where TDestination : class
    {
        return _dbSet.OfType<TSource>().ProjectTo<TDestination>(_mapper.ConfigurationProvider);
    }
}