using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.ObjectModel;

namespace Filters
{
    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            var allowAnonymousAttribute = context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>();
            if (false && allowAnonymousAttribute != null)
            {
                operation.Parameters.Insert(0, new OpenApiParameter { Name = "token", In = ParameterLocation.Header, Description = "Header", Required = true });
            }
        }
    }
}
