using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TechShop.Application.Services.TempDataService;

public class ControllerExceptionFilter : IActionFilter
{
    private readonly ITempDataService _tempDataService;

    public void OnActionExecuting(ActionExecutingContext context)
    {

    }

    public ControllerExceptionFilter(ITempDataService tempDataService)
    {
        _tempDataService = tempDataService;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            var referer = context.HttpContext.Request.Headers["Referer"].ToString();
            _tempDataService.Set("ErrorMessage", context.Exception.Message);

            context.Result = new RedirectResult(referer);
            context.ExceptionHandled = true;
        }
    }
}
