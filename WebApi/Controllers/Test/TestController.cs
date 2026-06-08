using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace WebApi.Controllers.Test
{
    public class TestController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> List()
        {
            return await Task.Run(() =>
            {
                return Ok("Connection Successful!!!");
            });
        }
    }
}