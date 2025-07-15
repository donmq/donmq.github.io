
using System.Linq.Expressions;

namespace Machine_API._Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindById(object id);

        Task<T> FindSingle(Expression<Func<T, bool>> predicate);

        IQueryable<T> FindAll();

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);

        void Add(T entity);

        void AddMultiple(List<T> entities);

        void Update(T entity);

        void UpdateMultiple(List<T> entities);

        void Remove(T entity);

        void Remove(object id);

        void RemoveMultiple(List<T> entities);

        bool All(Expression<Func<T, bool>> predicate);

        Task<bool> AllAsync(Expression<Func<T, bool>> predicate);

        bool Any();
        bool Any(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        T FirstOrDefault();
        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        int Count();
        int Count(Expression<Func<T, bool>> predicate);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        T LastOrDefault();
        T LastOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> LastOrDefaultAsync();
        Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate);

        decimal Sum(Expression<Func<T, decimal>> selector);
        decimal? Sum(Expression<Func<T, decimal?>> selector);
        decimal Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector);
        decimal? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector);

        Task<decimal> SumAsync(Expression<Func<T, decimal>> selector);
        Task<decimal?> SumAsync(Expression<Func<T, decimal?>> selector);
        Task<decimal> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector);
        Task<decimal?> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector);

        int Sum(Expression<Func<T, int>> selector);
        int? Sum(Expression<Func<T, int?>> selector);
        int Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector);
        int? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector);

        Task<int> SumAsync(Expression<Func<T, int>> selector);
        Task<int?> SumAsync(Expression<Func<T, int?>> selector);
        Task<int> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector);
        Task<int?> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector);

        long Sum(Expression<Func<T, long>> selector);
        long? Sum(Expression<Func<T, long?>> selector);
        long Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector);
        long? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector);

        Task<long> SumAsync(Expression<Func<T, long>> selector);
        Task<long?> SumAsync(Expression<Func<T, long?>> selector);
        Task<long> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector);
        Task<long?> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector);

        float Sum(Expression<Func<T, float>> selector);
        float? Sum(Expression<Func<T, float?>> selector);
        float Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector);
        float? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector);

        Task<float> SumAsync(Expression<Func<T, float>> selector);
        Task<float?> SumAsync(Expression<Func<T, float?>> selector);
        Task<float> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector);
        Task<float?> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector);
    }
}