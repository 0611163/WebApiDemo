using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using Utils;

namespace Filters
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写基类的异常处理方法
        /// </summary>
        public override Task OnExceptionAsync(ExceptionContext exceptionContext)
        {
            if (exceptionContext.ExceptionHandled == false)
            {
                exceptionContext.Result = new ObjectResult(new ApiResult
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = exceptionContext.Exception.Message,
                    Data = null
                });
                exceptionContext.ExceptionHandled = true; // 设置为true，表示异常已经被处理了
                LogUtil.Error(exceptionContext.Exception);
            }

            return base.OnExceptionAsync(exceptionContext);
        }
    }
}
