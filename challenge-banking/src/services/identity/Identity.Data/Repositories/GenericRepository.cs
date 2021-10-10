using Identity.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Identity.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        private readonly DbSet<T> DbSet;
        public GenericRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            DbSet = _context.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsEnumerable();
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = DbSet.Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }
        public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<T> GetSingleAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }

        public Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefaultAsync(predicate);
        }

        public T Update(T entity)
        {
            DbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual IQueryable<T> Query(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           int? page = null,
           int? pageSize = null,
           params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return query;
        }

        public virtual async Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return await query.ToListAsync();
        }

        ~GenericRepository()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            _context.Dispose();
        }
    }
}
