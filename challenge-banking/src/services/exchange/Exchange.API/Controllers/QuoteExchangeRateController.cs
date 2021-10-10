using AutoMapper;
using Exchange.API.Contracts.Requests;
using Exchange.API.Contracts.Responses;
using Exchange.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Exchange.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/quoteExchangeRate")]
    [ApiController]
    public class QuoteExchangeRateController : ControllerBase
    {
        private readonly ILogger<QuoteExchangeRateController> logger;
        private readonly IQuoteExchangeRateService service;
        private readonly IMapper mapper;
        public QuoteExchangeRateController(ILogger<QuoteExchangeRateController> logger, IMapper mapper, IQuoteExchangeRateService service)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.service = service;
        }

        [HttpPost]
        public async Task<ActionResult<QuoteExchangeRateResponse>> Post([FromBody] QuoteExchangeRateCreateRequest quoteExchangeRateCreateRequest)
        {
            var amount = quoteExchangeRateCreateRequest.Amount;
            var sourceCurrency = quoteExchangeRateCreateRequest.SourceCurrency;
            var targetCurrency = quoteExchangeRateCreateRequest.TargetCurrency;
            var resultQuoteExchangeRate = await service.CalculateQuoteExchangeRate(amount, sourceCurrency, targetCurrency);
            var response = mapper.Map<QuoteExchangeRateResponse>(resultQuoteExchangeRate);
            return Ok(response);

        }
    }
}
