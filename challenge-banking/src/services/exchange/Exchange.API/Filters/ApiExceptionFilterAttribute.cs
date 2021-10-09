using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.Common.Exceptions;
using System;

namespace Catalog.API.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute 
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                var ex = context.Exception as NotFoundException;
                context.Exception = null;
                var error = new
                {
                    Success = false,
                    Errors = new[] { ex.Message }
                };

                context.Result = new JsonResult(error);
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            }
            else if (context.Exception is BadRequestException)
            {
                var ex = context.Exception as BadRequestException;
                context.Exception = null;
                var error = new
                {
                    Success = false,
                    Errors = new[] { ex.Message }
                };
                context.Result = new JsonResult(error);
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                var error = new
                {
                    Success = false,
                    Errors = new[] { context.Exception.Message }
                };
                context.Result = new JsonResult(error);
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            }
            else if (context.Exception is ForbiddenException)
            {
                var error = new
                {
                    Success = false,
                    Errors = new[] { context.Exception.Message }
                };

                context.Result = new JsonResult(error);
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
            base.OnException(context);
        }
    }
}
