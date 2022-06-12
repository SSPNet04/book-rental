using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PagingExtensions
{
    public class PagingOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            MethodInfo methodInfo;
            context.ApiDescription.TryGetMethodInfo(out methodInfo);
            var paging = methodInfo.GetCustomAttribute<PagingResponseHeadersAttribute>();

            if (paging != null)
            {
                foreach (var responseCode in operation.Responses.Keys)
                {
                    if (responseCode == "200")
                    {
                        var response = operation.Responses[responseCode];
                        if (response.Headers == null)
                            response.Headers = new Dictionary<string, OpenApiHeader>();

                        response.Headers["X-Paging-PageNo"] = new OpenApiHeader
                        {
                            Schema = new OpenApiSchema { Type = "integer" },
                            Description = "Page No"
                        };
                        response.Headers["X-Paging-PageSize"] = new OpenApiHeader
                        {
                            Schema = new OpenApiSchema { Type = "integer" },
                            Description = "Page Size"
                        };
                        response.Headers["X-Paging-PageCount"] = new OpenApiHeader
                        {
                            Schema = new OpenApiSchema { Type = "integer" },
                            Description = "Paging Page Count"
                        };
                        response.Headers["X-Paging-TotalRecordCount"] = new OpenApiHeader
                        {
                            Schema = new OpenApiSchema { Type = "integer" },
                            Description = "Total Record Count"
                        };
                    }
                }
            }
        }
    }
}
