using Contracts.Responses.Route;
using Mediators.Route;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<RouteResponse>> Get(string source, string destination)
        {
            return await Mediator.Send(
                new GetRoute.Command
                {
                    Source = source,
                    Destination = destination
                });
        }
    }
}
