
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BuildBlocks.WebApi.Filters;

public class FormFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation == null || operation.RequestBody == null) return;

        foreach (var content in operation.RequestBody.Content.Where(x => x.Key.Equals("multipart/form-data", StringComparison.InvariantCultureIgnoreCase)))
        {
            var schema = content.Value.Schema;

            if (schema.Properties == null) continue;

            foreach (var prop in schema.Properties.Where(p => p.Value.Format == "binary"))
            {
                prop.Value.Type = "string";
                prop.Value.Format = "binary";
            }
        }
    }
}
