using AutoMapper;
using Identity.API.Contracts.Requests;
using Identity.API.Contracts.Responses;
using Identity.Domain.Models;
using Identity.Service;
using Identity.Service.ResourceParameters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

namespace Identity.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        public UserController(ILogger<UserController> logger, IMapper mapper, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult<DataCollection<UserResponse>>> Get([FromQuery] UserResourceParameters userResourceParameters, string ids = null)
        {
            IEnumerable<int> currencies = null;

            if (!string.IsNullOrEmpty(ids))
                currencies = ids.Split(',').Select(x => Convert.ToInt32(x));

            var model = await userService.GetUsers(userResourceParameters, currencies);
            Response.AddPagination(model.GetHeader().ToJson());

            var UserResponseModel = new DataCollection<UserResponse>()
            {
                Paging = model.GetHeader(),
                Links = GetLinks(userResourceParameters, "GetUsers", model.HasNextPage, model.HasPreviousPage),
                Items = model.Select(b => mapper.Map<UserResponse>(b))
            };
            return Ok(UserResponseModel);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponse>> Get([FromRoute] int id)
        {
            var User = await userService.GetUserById(id);
            if (User == null)
                throw new NotFoundException("User is not found");

            var reponse = mapper.Map<UserResponse>(User);
            return Ok(reponse);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<UserResponse>> Get([FromRoute] string email)
        {
            var user = await userService.GetUserByEmail(email);
            if (user == null)
                throw new NotFoundException("User is not found.");

            var response = mapper.Map<UserResponse>(user);
            return Ok(response);
        }

        //[HttpPost]
        //public async Task<ActionResult<UserResponse>> Post([FromBody] UserCreateRequest userCreateRequest)
        //{
        //    User entity = new User()
        //    {
        //        Email = userCreateRequest.Email
        //    };
        //    await userService.AddUser(entity, userCreateRequest.Password);
        //    UserResponse response = mapper.Map<UserResponse>(entity);
        //    return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        //}

        private IEnumerable<LinkInfo> GetLinks(UserResourceParameters userResourceParameters, string routeName, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkInfo>();
            links.Add(CreateLink(userResourceParameters, ResourceUriType.Current, routeName, "self", "GET"));

            if (hasNext)
                links.Add(CreateLink(userResourceParameters, ResourceUriType.NextPage, routeName, "nextPage", "GET"));

            if (hasPrevious)
                links.Add(CreateLink(userResourceParameters, ResourceUriType.PreviousPage, routeName, "previousPage", "GET"));

            return links;
        }

        private LinkInfo CreateLink(UserResourceParameters userResourceParameters, ResourceUriType type,
            string routeName, string rel, string method)
        {
            return new LinkInfo()
            {
                Href = CreateCurrencysResourceUri(routeName, userResourceParameters, type),
                Method = method,
                Rel = rel
            };
        }

        private string CreateCurrencysResourceUri(string routeName, UserResourceParameters userResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                      new
                      {
                          pageNumber = userResourceParameters.PageNumber - 1,
                          pageSize = userResourceParameters.PageSize,
                          searchQuery = userResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                      new
                      {
                          pageNumber = userResourceParameters.PageNumber + 1,
                          pageSize = userResourceParameters.PageSize,
                          searchQuery = userResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current:
                default:
                    return Url.Link(routeName,
                    new
                    {
                        pageNumber = userResourceParameters.PageNumber,
                        pageSize = userResourceParameters.PageSize,
                        searchQuery = userResourceParameters.SearchQuery
                    });
            }

        }
    }

}

