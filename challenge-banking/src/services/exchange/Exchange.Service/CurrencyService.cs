using Exchange.Data.Interfaces;
using Exchange.Domain.Models;
using Exchange.Service.ResourceParameters;
using Service.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Exchange.Service
{
    public interface ICurrencyService
    {
        Task<int> CountAsync(Expression<Func<Currency, bool>> predicate);
        Task<PagedList<Currency>> GetCurrencies(CurrencyResourceParameters currencyResourceParameters, IEnumerable<int> currencies = null);
        Task<Currency> AddCurrency(Currency currency);
        Task<Currency> GetCurrencyById(int id);
        Task<Currency> UpdateCurrency(Currency currency);
        Task<bool> DeleteCurrency(int id);
    }
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<Currency> currencyRepository;
        public CurrencyService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            currencyRepository = unitOfWork.GetRepository<Currency>();
        }
        public async Task<Currency> AddCurrency(Currency currency)
        {
            await currencyRepository.AddAsync(currency);
            await unitOfWork.SaveChangesAsync();
            return currency;
        }

        public async Task<int> CountAsync(Expression<Func<Currency, bool>> predicate)
        {
            return await currencyRepository.CountAsync(predicate);
        }

        public async Task<bool> DeleteCurrency(int id)
        {
            var currency = await currencyRepository.GetSingleAsync(id);
            currencyRepository.Delete(currency);
            return await unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedList<Currency>> GetCurrencies(CurrencyResourceParameters currencyResourceParameters, IEnumerable<int> currencies = null)
        {
            var collection = currencyRepository.Query();

            if (!string.IsNullOrWhiteSpace(currencyResourceParameters.SearchQuery))
            {
                var searchQuery = currencyResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery));
            }

            return await PagedList<Currency>.ToPagedListAsync(
                collection.Where(a => currencies == null || currencies.Contains(a.CurrencyId)).OrderBy(on => on.Code),
                currencyResourceParameters.PageNumber,
                currencyResourceParameters.PageSize);
        }

        public async Task<Currency> GetCurrencyById(int id)
        {
            return await currencyRepository.GetSingleAsync(id);
        }

        public async Task<Currency> UpdateCurrency(Currency currency)
        {
            currencyRepository.Update(currency);
            await unitOfWork.SaveChangesAsync();
            return currency;
        }
    }
}
