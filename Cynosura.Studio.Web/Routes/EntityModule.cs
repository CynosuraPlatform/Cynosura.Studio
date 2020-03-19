using Cynosura.Web.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Cynosura.Studio.Web.Routes
{
    public class EntityModule : IConfigurationModule<IEndpointRouteBuilder>
    {
        public void Configure(IEndpointRouteBuilder configuration)
        {
            configuration.MapGrpcService<Services.EntityService>();
        }
    }
}