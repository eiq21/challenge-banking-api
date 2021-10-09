using Exchange.Data.Interfaces;
using Exchange.Domain.Models;
using Exchange.Service.ResourceParameters;
using Service.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Service
{
    public interface IExchangeRateService
    {
        Task<int> CountAsync(Expression<Func<ExchangeRate, bool>> predicate);
        Task<PagedList<ExchangeRate>> GetExchangeRates(ExchangeRateResourceParameters typeChangeResourceParameters, IEnumerable<int> typeChanges = null);
        Task<ExchangeRate> AddExchangeRate(ExchangeRate exchangeRate);
        Task<ExchangeRate> GetExchangeRateById(int id);
        Task<ExchangeRate> UpdateExchangeRate(ExchangeRate exchangeRate);
        Task<bool> DeleteExchangeRate(int id);
    }

    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<ExchangeRate> exchangeRateRepository;
        public ExchangeRateService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            exchangeRateRepository = unitOfWork.GetRepository<ExchangeRate>();
        }
        public async Task<ExchangeRate> AddExchangeRate(ExchangeRate exchangeRate)
        {
            await exchangeRateRepository.AddAsync(exchangeRate);
            await unitOfWork.SaveChangesAsync();
            return exchangeRate;
        }

        public async Task<int> CountAsync(Expression<Func<ExchangeRate, bool>> predicate)
        {
            return await exchangeRateRepository.CountAsync(predicate);
        }

        public async Task<bool> DeleteExchangeRate(int id)
        {
            var ExchangeRate = await exchangeRateRepository.GetSingleAsync(id);
            exchangeRateRepository.Delete(ExchangeRate);
            return await unitOfWork.SaveChangesAsync();
        }

        public async Task<ExchangeRate> GetExchangeRateById(int id)
        {
            return await exchangeRateRepository.GetSingleAsync(id);
        }

        public async Task<PagedList<ExchangeRate>> GetExchangeRates(ExchangeRateResourceParameters ExchangeRateResourceParameters, IEnumerable<int> ExchangeRates = null)
        {
            var collection = exchangeRateRepository.Query();

            if (!string.IsNullOrWhiteSpace(ExchangeRateResourceParameters.SearchQuery))
            {
                var searchQuery = ExchangeRateResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.Pair.Contains(searchQuery));
            }

            return await PagedList<ExchangeRate>.ToPagedListAsync(
                collection.Where(a => ExchangeRates == null || ExchangeRates.Contains(a.ExchangeRateId)).OrderBy(on => on.ExchangeRateAt),
                ExchangeRateResourceParameters.PageNumber,
                ExchangeRateResourceParameters.PageSize);
        }

        public async Task<ExchangeRate> UpdateExchangeRate(ExchangeRate exchangeRate)
        {
            exchangeRateRepository.Update(exchangeRate);
            await unitOfWork.SaveChangesAsync();
            return exchangeRate;
        }
    }
}
