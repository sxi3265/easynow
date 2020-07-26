using EasyNow.Dal;
using EasyNow.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyNow.Api.Filters
{
    public class ResultActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                context.Result = new ObjectResult(new Result<object>{ Data = objectResult.Value });
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new Result());
            }
            else if (context.Result is ContentResult contentResult)
            {
                context.Result = new ObjectResult(new Result<object>{ Data = contentResult.Content });
            }
            else if (context.Result is StatusCodeResult)
            {
                context.Result = new ObjectResult(new Result());
            }
            else if (context.Result is JsonResult jsonResult)
            {
                context.Result = new ObjectResult(new Result<object>{ Data = jsonResult.Value });
            }
        }
    }
}