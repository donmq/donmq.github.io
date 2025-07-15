using System.Linq.Expressions;
using Machine_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Repositories
{
    public class SAPRepository<T> : IRepository<T> where T : class
    {
        private readonly SAPContext _context;
        public SAPRepository(SAPContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void AddMultiple(List<T> entities)
        {
            _context.AddRange(entities);
        }

        public IQueryable<T> FindAll()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public async Task<T> FindById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> FindSingle(Expression<Func<T, bool>> predicate)
        {
            return await FindAll().FirstOrDefaultAsync(predicate);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Remove(object id)
        {
            Remove(FindById(id));
        }

        public void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateMultiple(List<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public bool All(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().All(predicate);
        }

        public async Task<bool> AllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AllAsync(predicate);
        }

        public bool Any()
        {
            return _context.Set<T>().Any();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.Set<T>().AnyAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public T FirstOrDefault()
        {
            return _context.Set<T>().FirstOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            return await _context.Set<T>().FirstOrDefaultAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Count(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }

        public T LastOrDefault()
        {
            return _context.Set<T>().LastOrDefault();
        }

        public T LastOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().LastOrDefault(predicate);
        }

        public async Task<T> LastOrDefaultAsync()
        {
            return await _context.Set<T>().LastOrDefaultAsync();
        }

        public async Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().LastOrDefaultAsync(predicate);
        }

        public decimal Sum(Expression<Func<T, decimal>> selector)
        {
            return _context.Set<T>().Sum(selector);
        }

        public decimal? Sum(Expression<Func<T, decimal?>> selector)
        {
            return _context.Set<T>().Sum(selector);
        }

        public decimal Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector)
        {
            return _context.Set<T>().Where(predicate).Sum(selector);
        }

        public decimal? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector)
        {
            return _context.Set<T>().Where(predicate).Sum(selector);
        }

        public async Task<decimal> SumAsync(Expression<Func<T, decimal>> selector)
        {
            return await _context.Set<T>().SumAsync(selector);
        }

        public async Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector)
        {
            return await _context.Set<T>().SumAsync(selector);
        }

        public async Task<decimal> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector)
        {
            return await _context.Set<T>().Where(predicate).SumAsync(selector);
        }

        public async Task<decimal?> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector)
        {
            return await _context.Set<T>().Where(predicate).SumAsync(selector);
        }

        public int Sum(Expression<Func<T, int>> selector)
        {
            return _context.Set<T>().Sum(selector);
        }

        public int? Sum(Expression<Func<T, int?>> selector)
        {
            return _context.Set<T>().Sum(selector);
        }

        public int Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector)
        {
            return _context.Set<T>().Where(predicate).Sum(selector);
        }

        public int? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector)
        {
            return _context.Set<T>().Where(predicate).Sum(selector);
        }

        public async Task<int> SumAsync(Expression<Func<T, int>> selector)
        {
            return await _context.Set<T>().SumAsync(selector);
        }

        public async Task<int?> SumAsync(Expression<Func<T, int?>> selector)
        {
            return await _context.Set<T>().SumAsync(selector);
        }

        public async Task<int> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector)
        {
            return await _context.Set<T>().Where(predicate).SumAsync(selector);
        }

        public async Task<int?> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector)
        {
            return await _context.Set<T>().Where(predicate).SumAsync(selector);
        }

        public long Sum(Expression<Func<T, long>> selector)
        {
            return _context.Set<T>().Sum(selector);
        }

        public long? Sum(Expression<Func<T, long?>> selector)
        {
            return _context.Set<T>().Sum(selector);
        }

        public long Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector)
        {
            return _context.Set<T>().Where(predicate).Sum(selector);
        }

        public long? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector)
        {
            return _context.Set<T>().Where(predicate).Sum(selector);
        }

        public async Task<long> SumAsync(Expression<Func<T, long>> selector)
        {
            return await _context.Set<T>().SumAsync(selector);
        }

        public async Task<long?> SumAsync(Expression<Func<T, long?>> selector)
        {
            return await _context.Set<T>().SumAsync(selector);
        }

        public async Task<long> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector)
        {
            return await _context.Set<T>().Where(predicate).SumAsync(selector);
        }

        public async Task<long?> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector)
        {
            return await _context.Set<T>().Where(predicate).SumAsync(selector);
        }

        public float Sum(Expression<Func<T, float>> selector)
        {
            return _context.Set<T>().Sum(selector);
        }

        public float? Sum(Expression<Func<T, float?>> selector)
        {
            return _context.Set<T>().Sum(selector);
        }

        public float Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector)
        {
            return _context.Set<T>().Where(predicate).Sum(selector);
        }

        public float? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector)
        {
            return _context.Set<T>().Where(predicate).Sum(selector);
        }

        public async Task<float> SumAsync(Expression<Func<T, float>> selector)
        {
            return await _context.Set<T>().SumAsync(selector);
        }

        public async Task<float?> SumAsync(Expression<Func<T, float?>> selector)
        {
            return await _context.Set<T>().SumAsync(selector);
        }

        public async Task<float> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector)
        {
            return await _context.Set<T>().Where(predicate).SumAsync(selector);
        }

        public async Task<float?> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector)
        {
            return await _context.Set<T>().Where(predicate).SumAsync(selector);
        }
    }
}