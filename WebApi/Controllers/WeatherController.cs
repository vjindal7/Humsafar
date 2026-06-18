using Contracts.Responses.Weather;
using Mediators.Weather;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : BaseController
    {
        [HttpGet("{city}")]
        public async Task<ActionResult<WeatherResponse>> Get(string city)
        {
            return await Mediator.Send(new GetWeather.Command
            {
                City = city
            });
        }
    }
}
