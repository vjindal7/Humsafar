using Contracts.Requests.Travel;
using Contracts.Responses.Travel;
using Mediators.Travel;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class TravelController : BaseController
    {
        [HttpPost("ask")]
        public async Task<ActionResult<TravelResponse>> Ask(TravelRequest request)
        {
            return await Mediator.Send(new Ask.Command());
        }

        [HttpPost("attractions")]
        public async Task<ActionResult<AttractionResponse>> GetAttractions(AttractionRequest request)
        {
            return await Mediator.Send(new Attractions.Command
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Mood = request.Mood,
                Preferences = request.Preferences
            });
        }
    }
}
