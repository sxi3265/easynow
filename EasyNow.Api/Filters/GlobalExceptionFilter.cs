using System;
using EasyNow.Dal;
using EasyNow.Dto;
using EasyNow.Utility.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EasyNow.Api.Filters
{
    public class GlobalExceptionFilter: IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILoggerProvider>().CreateLogger(context.ActionDescriptor.DisplayName);
            var environment = context.HttpContext.RequestServices.GetService<IHostEnvironment>();

            // 标记该异常已被处理
            context.ExceptionHandled = true;
            if (context.Exception is MessageException exception)
            {
                context.Result = new ObjectResult(new
                    Result<Object>{ Code = -1, Msg = exception.Message, Data = exception.Data });
            }
            else
            {
                logger.LogError(context.Exception, string.Empty);
                context.Result = environment.IsDevelopment() ? new ObjectResult(new Result<Exception>{ Code = -1, Msg = "请求发生错误", Data = context.Exception }) : new ObjectResult(new Result{ Code = -1, Msg = "请求发生错误" });
            }
        }
    }
}