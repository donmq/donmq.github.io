using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace eTierV2_API._Repositories.Repositories
{
    public class EfficiencyRepository<T> : Repository<T>, IEfficiencyRepository<T> where T : class
    {
        private IConfiguration _configuration;

        public EfficiencyRepository(DBContext context, IConfiguration configuration) : 
            base(context, configuration)
        {
            _configuration = configuration;
        }

        public IQueryable<T> FindAllContext(DBContext context, params Expression<Func<T, object>>[] includeProperties)
        {
            DataSeach = _configuration.GetSection("AppSettings:DataSeach").Value;
            IQueryable<T> items = null;

            items = context.Set<T>();

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }

            return items.AsNoTracking().AsQueryable();
        }

        public IQueryable<T> FindAllContext(DBContext context, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            DataSeach = _configuration.GetSection("AppSettings:DataSeach").Value;
            IQueryable<T> items = null;
       
            items = context.Set<T>();

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }

            return items.Where(predicate).AsNoTracking().AsQueryable();
        }
    }
}