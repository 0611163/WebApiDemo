using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using Utils;

namespace Filters
{
    /// <summary>
    /// 接口限速
    /// </summary>
    public class RateLimitFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Type controllerType = context.Controller.GetType();
            if (context.ActionArguments.Count == 0)
            {
                await next();
                return;
            }
            object arg = context.ActionArguments.Values.ToList()[0];
            var rateLimit = context.ActionDescriptor.EndpointMetadata.OfType<RateLimitAttribute>().FirstOrDefault();

            bool isLimit = false;
            if (rateLimit != null && arg is RateLimitInterface)
            {
                RateLimitInterface model = arg as RateLimitInterface;
                if (model.IsLimit) //满足限速条件
                {
                    isLimit = true;
                    SemaphoreSlim sem = rateLimit.Sem;

                    if (await sem.WaitAsync(rateLimit.Timeout))
                    {
                        try
                        {
                            await next();
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            sem.Release();
                        }
                    }
                    else
                    {
                        var routeList = context.RouteData.Values.Values.ToList();
                        routeList.Reverse();
                        var route = string.Join('/', routeList.ConvertAll(a => a.ToString()));
                        var msg = $"当前访问{route}接口的用户数太多，请稍后再试";
                        LogUtil.Info(msg);
                        context.Result = new ObjectResult(new ApiResult
                        {
                            Code = (int)HttpStatusCode.ServiceUnavailable,
                            Message = "当前查询的用户太多，请稍后再试。"
                        });
                    }
                }
            }

            if (!isLimit)
            {
                await next();
            }
        }
    }
}
