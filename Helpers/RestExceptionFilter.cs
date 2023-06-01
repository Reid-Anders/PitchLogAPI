using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PitchLogAPI.Helpers
{
    public class ResourceNotFoundExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Exception is ResourceNotFoundException ex)
            {
                context.Result = new NotFoundResult();
                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
