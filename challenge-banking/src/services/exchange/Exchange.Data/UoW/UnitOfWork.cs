using Exchange.Data.Interfaces;
using Exchange.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exchange.Data.UoW
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, IDisposable
    {
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        public Dictionary<Type, object> Repositories { 
            get { return _repositories; } 
            set { Repositories = value; } 
        }

        public TContext Context { get; }
        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IGenericRepository<T>;
            }

            IGenericRepository<T> repo = new GenericRepository<T>(Context);
            Repositories.Add(typeof(T), repo);
            return repo;
        }

        ~UnitOfWork() {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposing) return;
            Context.Dispose();
        }
    }
}
