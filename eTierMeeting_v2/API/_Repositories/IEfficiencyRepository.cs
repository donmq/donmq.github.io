using eTierV2_API._Repositories.Repositories;
using eTierV2_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace eTierV2_API._Repositories.Interfaces
{
    public interface IEfficiencyRepository<T> : IRepository<T> where T : class
    {
        IQueryable<T> FindAllContext(DBContext context, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindAllContext(DBContext context, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
    }
}