using Exchange.Data.Interfaces;
using Exchange.Domain.Helpers;
using Exchange.Domain.Models;
using Service.Common.Exceptions;
using System.Threading.Tasks;

namespace Exchange.Service
{
    public interface IQuoteExchangeRateService
    {
        Task<QuoteExchangeRate> CalculateQuoteExchangeRate(decimal amount, string sourceCurrency, string targetCurrency);
    }
    public class QuoteExchangeRateService : IQuoteExchangeRateService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<ExchangeRate> exchangeRateRepository;
        public QuoteExchangeRateService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            exchangeRateRepository = unitOfWork.GetRepository<ExchangeRate>();
        }
        public async Task<QuoteExchangeRate> CalculateQuoteExchangeRate(decimal amount, string sourceCurrency, string targetCurrency)
        {
            var exchangeRate = await exchangeRateRepository.GetSingleAsync(a => a.ExchangeRateAt.Date == System.DateTime.Now.Date && a.IsActive == true);
            if (exchangeRate == null)
                throw new NotFoundException("Unregistered exchange rate of the day");

            bool isDemand = targetCurrency == "USD";
            var fixingRate = isDemand ? exchangeRate.Demand : exchangeRate.Offer;
            var targetAmount = isDemand ? System.Math.Round(amount / exchangeRate.Demand, 2) : System.Math.Round(amount * exchangeRate.Offer, 2);

            var quoteExchangeRate = new QuoteExchangeRate
            {
                SourceCurrency = sourceCurrency,
                TargetCurrency = targetCurrency,
                SourceAmount = amount,
                TargetAmount = targetAmount,
                FixingExchangeRate = fixingRate,
            };

            return quoteExchangeRate;
        }

    }
}
