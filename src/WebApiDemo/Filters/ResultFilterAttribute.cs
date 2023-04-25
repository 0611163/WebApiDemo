using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Newtonsoft.Json;
using System.Net;

namespace Filters
{
    public class ResultFilterAttribute : Attribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objResult)
            {
                if (objResult.Value is ApiResult) return;

                context.Result = new ObjectResult(new ApiResult
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = string.Empty,
                    Data = objResult.Value
                });
            }
        }
    }
}
