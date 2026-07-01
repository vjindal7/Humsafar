using Contracts.Requests.Traffic;
using Contracts.Responses.Traffic;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrafficController : BaseController
    {
        private readonly ITrafficService _trafficService;

        public TrafficController(ITrafficService trafficService)
        {
            _trafficService = trafficService;
        }

        [HttpGet("gettraffic")]
        public async Task<ActionResult<TrafficResponse>> GetTrafficDetails([FromQuery] TrafficRequest request)
        {
            var result = await _trafficService.GetTrafficDetailsAsync(
                request.SourceLat,
                request.SourceLng,
                request.DestinationLat,
                request.DestinationLng,
                request.TravelMode);

            using var json = JsonDocument.Parse(result);

            var summary = json.RootElement
                .GetProperty("routes")[0]
                .GetProperty("summary");

            return Ok(new TrafficResponse
            {
                Distance = summary.GetProperty("lengthInMeters").GetInt32(),
                TravelTime = summary.GetProperty("travelTimeInSeconds").GetInt32(),
                TrafficDelay = summary.GetProperty("trafficDelayInSeconds").GetInt32(),
                NoTrafficTime = summary.GetProperty("noTrafficTravelTimeInSeconds").GetInt32(),
                ArrivalTime = summary.GetProperty("arrivalTime").GetString() ?? string.Empty
            });
        }
    }
}
