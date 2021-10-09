using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Exchange.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<bool> SaveChangesAsync();
        bool SaveChanges();
    }

    public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}
