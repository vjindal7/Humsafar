using Contracts.Requests.Travel;
using Contracts.Responses.Route;
using Contracts.Responses.Travel;

using Mediators.Travel;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
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

        [HttpPost("recommendations")]
        public async Task<ActionResult<List<StopResponse>>> GetRecommendations(Recommendations.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}
