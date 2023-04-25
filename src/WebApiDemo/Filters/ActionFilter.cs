using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using System.Diagnostics;
using System.Net;
using Utils;

namespace Filters
{
    public class ActionFilter : Attribute, IAsyncActionFilter, IAsyncResultFilter
    {
        private Stopwatch _stopwatch = Stopwatch.StartNew();

        private IConfiguration _configuration;

        public ActionFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //token验证
            var allowAnonymousAttribute = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().FirstOrDefault();
            if (allowAnonymousAttribute == null)
            {
                string token = null;
                if (context.HttpContext.Request.Headers.ContainsKey(nameof(token)))
                {
                    token = context.HttpContext.Request.Headers[nameof(token)].ToString();
                }

                if (token != null)
                {
                    var tokenConfig = _configuration.GetSection("token").Value;
                    if (token != tokenConfig)
                    {
                        context.Result = new ObjectResult(new ApiResult
                        {
                            Code = (int)HttpStatusCode.BadRequest,
                            Message = "token不正确"
                        });
                        return;
                    }
                }
                else
                {
                    context.Result = new ObjectResult(new ApiResult
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "需要token"
                    });
                    return;
                }
            }

            await next();
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            //耗时较长的接口输出日志
            _stopwatch.Stop();
            double time = _stopwatch.Elapsed.TotalSeconds;
            if (time > 0)
            {
                var routeList = context.RouteData.Values.Values.ToList().ConvertAll(a => a.ToString());
                routeList.Reverse();
                var route = string.Join('/', routeList);
                var msg = $"接口 {route} 耗时：{time:0.000} 秒";
                Console.WriteLine(msg);
                LogUtil.Info(msg);
            }

            await next();
        }
    }
}
