using AutoMapper;
using Exchange.API.Contracts.Requests;
using Exchange.API.Contracts.Responses;
using Exchange.Domain.Models;
using Exchange.Service;
using Exchange.Service.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Common.Collection;
using Service.Common.Enums;
using Service.Common.Exceptions;
using Service.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exchange.API.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ILogger<CurrencyController> logger;
        private readonly ICurrencyService currencyService;
        private readonly IMapper mapper;
        public CurrencyController(ILogger<CurrencyController> logger, IMapper mapper, ICurrencyService currencyService)
        {
            this.logger = logger;
            this.currencyService = currencyService;
            this.mapper = mapper;
        }

        [HttpGet(Name = "GetCurrencies")]
        public async Task<ActionResult<DataCollection<CurrencyResponse>>> Get([FromQuery] CurrencyResourceParameters currencyResourceParameters, string ids = null)
        {
            IEnumerable<int> currencies = null;

            if (!string.IsNullOrEmpty(ids))
                currencies = ids.Split(',').Select(x => Convert.ToInt32(x));

            var model = await currencyService.GetCurrencies(currencyResourceParameters, currencies);
            Response.AddPagination(model.GetHeader().ToJson());

            var currencyResponseModel = new DataCollection<CurrencyResponse>()
            {
                Paging = model.GetHeader(),
                Links = GetLinks(currencyResourceParameters, "GetCurrencies", model.HasNextPage, model.HasPreviousPage),
                Items = model.Select(b => mapper.Map<CurrencyResponse>(b))
            };
            return Ok(currencyResponseModel);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CurrencyResponse>> Get([FromRoute] int id)
        {
            var Currency = await currencyService.GetCurrencyById(id);
            if (Currency == null)
                throw new NotFoundException("Currency is not found");

            var reponse = mapper.Map<CurrencyResponse>(Currency);
            return Ok(reponse);
        }

        [HttpPost]
        public async Task<ActionResult<CurrencyResponse>> Post([FromBody] CurrencyCreateRequest currencyCreateRequest)
        {
            Currency entity = new Currency()
            {
                Name = currencyCreateRequest.Name,
                Symbol = currencyCreateRequest.Symbol,
                Code = currencyCreateRequest.Code,
                CreatedBy = currencyCreateRequest.CreatedBy

            };
            await currencyService.AddCurrency(entity);
            CurrencyResponse response = mapper.Map<CurrencyResponse>(entity);
            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CurrencyResponse>> Put([FromRoute] int id, [FromBody] CurrencyUpdateRequest currencyUpdateRequest)
        {
            var currency = await currencyService.GetCurrencyById(id);
            if (currency == null)
                throw new NotFoundException("Currency is not found");

            currency.Name = currencyUpdateRequest.Name;
            currency.Symbol = currencyUpdateRequest.Symbol;
            currency.Code = currencyUpdateRequest.Code;
            currency.UpdatedBy = currencyUpdateRequest.UpdatedBy;
            currency.UpdatedAt = DateTime.Now;

            await currencyService.UpdateCurrency(currency);
            CurrencyResponse response = mapper.Map<CurrencyResponse>(currency);
            return Ok(response);
        }
        private IEnumerable<LinkInfo> GetLinks(CurrencyResourceParameters currencyResourceParameters, string routeName, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkInfo>();
            links.Add(CreateLink(currencyResourceParameters, ResourceUriType.Current, routeName, "self", "GET"));

            if (hasNext)
                links.Add(CreateLink(currencyResourceParameters, ResourceUriType.NextPage, routeName, "nextPage", "GET"));

            if (hasPrevious)
                links.Add(CreateLink(currencyResourceParameters, ResourceUriType.PreviousPage, routeName, "previousPage", "GET"));

            return links;
        }

        private LinkInfo CreateLink(CurrencyResourceParameters currencyResourceParameters, ResourceUriType type,
            string routeName, string rel, string method)
        {
            return new LinkInfo()
            {
                Href = CreateCurrencysResourceUri(routeName, currencyResourceParameters, type),
                Method = method,
                Rel = rel
            };
        }

        private string CreateCurrencysResourceUri(string routeName, CurrencyResourceParameters currencyResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                      new
                      {
                          pageNumber = currencyResourceParameters.PageNumber - 1,
                          pageSize = currencyResourceParameters.PageSize,
                          searchQuery = currencyResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                      new
                      {
                          pageNumber = currencyResourceParameters.PageNumber + 1,
                          pageSize = currencyResourceParameters.PageSize,
                          searchQuery = currencyResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current:
                default:
                    return Url.Link(routeName,
                    new
                    {
                        pageNumber = currencyResourceParameters.PageNumber,
                        pageSize = currencyResourceParameters.PageSize,
                        searchQuery = currencyResourceParameters.SearchQuery
                    });
            }

        }
    }
}
