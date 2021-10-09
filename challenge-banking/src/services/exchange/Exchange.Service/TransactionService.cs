using Exchange.Domain.Models;
using Exchange.Service.ResourceParameters;
using Service.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Exchange.Service
{
    public interface ITransactionService
    {
        Task<int> CountAsync(Expression<Func<Transaction, bool>> predicate);
        Task<PagedList<Currency>> GetTransactions(TransactionResourceParameters transactionResourceParameters, IEnumerable<int> currencies = null);
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<Transaction> GetTransactionById(int id);
        Task<Transaction> UpdateTransaction(Transaction transaction);
        
    }

    public class TransactionService : ITransactionService
    {
        public Task<Transaction> AddTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<Transaction, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetTransactionById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Currency>> GetTransactions(TransactionResourceParameters transactionResourceParameters, IEnumerable<int> currencies = null)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> UpdateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
