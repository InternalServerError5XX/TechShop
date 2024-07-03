using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace TechShopWeb.Filters
{
    public class ApiControllerExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Error = context.Exception.Message,
                Trace = context.Exception.StackTrace
            });

            context.ExceptionHandled = true;
        }
    }
}