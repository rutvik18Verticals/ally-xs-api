using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Theta.XSPOC.Apex.Api.RealTimeData
{
    public class CustomHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation?.Parameters.Add(new OpenApiParameter()
            {
                Name = "UserAccount",
                In = ParameterLocation.Header,
                Description = "Please enter your UserAccount",
                Required = false
            });

        }
    }
}