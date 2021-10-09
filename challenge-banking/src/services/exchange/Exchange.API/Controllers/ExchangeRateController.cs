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
    [Route("api/exchangeRates")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ILogger<ExchangeRateController> logger;
        private readonly IExchangeRateService exchangeRateService;
        private readonly IMapper mapper;
        public ExchangeRateController(ILogger<ExchangeRateController> logger, IMapper mapper, IExchangeRateService exchangeRateService)
        {
            this.logger = logger;
            this.exchangeRateService = exchangeRateService;
            this.mapper = mapper;
        }

        [HttpGet(Name = "GetExchangeRates")]
        public async Task<ActionResult<DataCollection<ExchangeRateResponse>>> Get([FromQuery] ExchangeRateResourceParameters typeChangeResourceParameters, string ids = null)
        {
            IEnumerable<int> exchanges = null;

            if (!string.IsNullOrEmpty(ids))
                exchanges = ids.Split(',').Select(x => Convert.ToInt32(x));

            var model = await exchangeRateService.GetExchangeRates(typeChangeResourceParameters, exchanges);
            Response.AddPagination(model.GetHeader().ToJson());

            var typeChangeResponseModel = new DataCollection<ExchangeRateResponse>()
            {
                Paging = model.GetHeader(),
                Links = GetLinks(typeChangeResourceParameters, "GetExchangeRates", model.HasNextPage, model.HasPreviousPage),
                Items = model.Select(b => mapper.Map<ExchangeRateResponse>(b))
            };
            return Ok(typeChangeResponseModel);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExchangeRateResponse>> Get([FromRoute] int id)
        {
            var exchangeRate = await exchangeRateService.GetExchangeRateById(id);
            if (exchangeRate == null)
                throw new NotFoundException("Currency is not found");

            var reponse = mapper.Map<ExchangeRateResponse>(exchangeRate);
            return Ok(reponse);
        }

        [HttpPost]
        public async Task<ActionResult<ExchangeRateResponse>> Post([FromBody] ExchangeRateCreateRequest exchangeRateCreateRequest)
        {
            ExchangeRate entity = new ExchangeRate()
            {
                Pair = exchangeRateCreateRequest.Pair,
                Offer = exchangeRateCreateRequest.Offer,
                Demand = exchangeRateCreateRequest.Demand,
                CreatedBy = exchangeRateCreateRequest.CreatedBy

            };
            await exchangeRateService.AddExchangeRate(entity);
            ExchangeRateResponse response = mapper.Map<ExchangeRateResponse>(entity);
            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ExchangeRateResponse>> Put([FromRoute] int id, [FromBody] ExchangeRateUpdateRequest exchangeRateUpdateRequest)
        {
            var exchangeRate = await exchangeRateService.GetExchangeRateById(id);
            if (exchangeRate == null)
                throw new NotFoundException("ExchangeRate is not found");

            exchangeRate.Offer = exchangeRateUpdateRequest.Offer;
            exchangeRate.Demand = exchangeRateUpdateRequest.Demand;
            exchangeRate.UpdatedBy = exchangeRateUpdateRequest.UpdatedBy;
            exchangeRate.UpdatedAt = DateTime.Now;

            await exchangeRateService.UpdateExchangeRate(exchangeRate);
            ExchangeRateResponse response = mapper.Map<ExchangeRateResponse>(exchangeRate);
            return Ok(response);
        }
        private IEnumerable<LinkInfo> GetLinks(ExchangeRateResourceParameters typeChangeResourceParameters, string routeName, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkInfo>();
            links.Add(CreateLink(typeChangeResourceParameters, ResourceUriType.Current, routeName, "self", "GET"));

            if (hasNext)
                links.Add(CreateLink(typeChangeResourceParameters, ResourceUriType.NextPage, routeName, "nextPage", "GET"));

            if (hasPrevious)
                links.Add(CreateLink(typeChangeResourceParameters, ResourceUriType.PreviousPage, routeName, "previousPage", "GET"));

            return links;
        }

        private LinkInfo CreateLink(ExchangeRateResourceParameters typeChangeResourceParameters, ResourceUriType type,
            string routeName, string rel, string method)
        {
            return new LinkInfo()
            {
                Href = CreateCurrencysResourceUri(routeName, typeChangeResourceParameters, type),
                Method = method,
                Rel = rel
            };
        }

        private string CreateCurrencysResourceUri(string routeName, ExchangeRateResourceParameters typeChangeResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                      new
                      {
                          pageNumber = typeChangeResourceParameters.PageNumber - 1,
                          pageSize = typeChangeResourceParameters.PageSize,
                          searchQuery = typeChangeResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                      new
                      {
                          pageNumber = typeChangeResourceParameters.PageNumber + 1,
                          pageSize = typeChangeResourceParameters.PageSize,
                          searchQuery = typeChangeResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current:
                default:
                    return Url.Link(routeName,
                    new
                    {
                        pageNumber = typeChangeResourceParameters.PageNumber,
                        pageSize = typeChangeResourceParameters.PageSize,
                        searchQuery = typeChangeResourceParameters.SearchQuery
                    });
            }

        }
    }
}
