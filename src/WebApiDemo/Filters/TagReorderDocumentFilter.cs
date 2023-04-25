using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Utils;

// 解决swagger控制器排序问题

namespace Filters
{
    /// <summary>
    /// Workaround for https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2162
    /// When adding XML controller descriptions to the swagger description document,
    /// controllers are listed out of alphabetical order.
    ///
    /// This filter explicitly reorders them.
    /// </summary>
    public class TagReorderDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Allows customization of the swagger description document
        /// </summary>
        /// <param name="swaggerDoc">The generated swagger description document</param>
        /// <param name="context">Context information</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags.Clear();

            foreach (var path in swaggerDoc.Paths)
            {
                foreach (var o in path.Value.Operations)
                {
                    foreach (var tag in o.Value.Tags)
                    {
                        swaggerDoc.Tags.Add(tag);
                    }
                }
            }

            swaggerDoc.Tags = swaggerDoc.Tags
                .OrderBy(tag => TinyPinYinUtil.GetPinYin(tag.Name))
                .ToList();
        }
    }
}
