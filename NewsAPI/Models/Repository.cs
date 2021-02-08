using Microsoft.EntityFrameworkCore;
using NewsAPI.Models.Pagging;
using NewsAPI.Models.Parameters;
using NewsAppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NewsAPI.Models
{
    public class Repository<TContext> : IRepository where TContext : DbContext
    {
        protected TContext dbContext;

        public Repository(TContext context)
        {
            dbContext = context;
        }

        public async Task CreateAsync<T>(T entity) where T : class
        {

            this.dbContext.Set<T>().Add(entity);
            _ = await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            this.dbContext.Set<T>().Remove(entity);

            _ = await this.dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> SelectAll<T>() where T : class
        {
            return await this.dbContext.Set<T>().ToListAsync();
        }

        public async Task<PagedList<T>> GetAllAsync<T>(PageParameters pageParamers, Expression<Func<T, bool>> searchTermPredicate, Expression<Func<T, object>> orderByPredicate) where T : class
        {
            var dbSet = dbContext.Set<T>().AsQueryable();
            if (typeof(T) == typeof(Article))
                dbSet = dbSet.Include("Writer");
            return await Task.FromResult(PagedList<T>.ToPagedList(dbSet.OrderByDescending(orderByPredicate).Where(searchTermPredicate),
                     pageParamers.PageNumber,
                         pageParamers.PageSize));
        }

        public async Task<T> SelectById<T>(object id) where T : class
        {
            return await this.dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            this.dbContext.Set<T>().Update(entity);
            _ = await this.dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> Find<T>(Expression<Func<T, bool>> searchTermPredicate) where T : class
        {
            return await this.dbContext.Set<T>().Where(searchTermPredicate).ToListAsync();
        }
    }
}
