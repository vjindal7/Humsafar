using Contracts.Requests.Traffic;
using Contracts.Responses.Traffic;
using Mediators.Traffic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrafficController : BaseController
    {
        [HttpGet("gettraffic")]
        public async Task<ActionResult<TrafficResponse>> GetTrafficDetails(TrafficRequest request)
        {
            return await Mediator.Send(new GetTraffic.Command
            {
                Source = request.Source,
                Destination = request.Destination
            });
        }
    }
}
