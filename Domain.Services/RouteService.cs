using Contracts.Responses.Route;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IRouteService
    {
        Task<RouteResponse> GenerateAsync(string source, string destination);
    }
    public class RouteService : IRouteService
    {
        public async Task<RouteResponse> GenerateAsync(string source, string destination)
        {
            await Task.Delay(300);

            return new RouteResponse
            {
                Source = source,
                Destination = destination,
                Distance = "280 km",
                Duration = "6h",
                Stops = new()
                {
                    "Cafe",
                    "View Point"
                }
            };
        }
    }
}
