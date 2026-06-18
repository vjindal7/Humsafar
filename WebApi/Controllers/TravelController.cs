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

        //[HttpGet("toptovisit")]
        //public async Task<ActionResult<TravelResponse>> Ask(TravelRequest request)
        //{
        //    return await Mediator.Send(new Ask.Command());
        //}
    }
}