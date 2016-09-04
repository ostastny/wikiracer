using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace wikiracer
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {    
        public override void OnException(ExceptionContext context)
        {
            if (context.HttpContext.Request.GetTypedHeaders().Accept.Any(header => header.MediaType == "application/json"))
            {
                var jsonResult = new JsonResult(new { error = context.Exception.Message });
                jsonResult.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;
                context.Result = jsonResult;
            }
            else
            {
                var plainResult = new ContentResult() { Content = context.Exception.Message };
                plainResult.ContentType = "text/plain";
                plainResult.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;
                context.Result = plainResult; 
            }
        }
    }
}