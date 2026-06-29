using Contracts.Requests.Route;
using Contracts.Responses.Route;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
    public interface IRecommendationService
    {
        List<StopResponse> GetStops(RouteRequest request);
    }
    public class RecommendationService : IRecommendationService
    {
        public List<StopResponse> GetStops(RouteRequest request)
        {
            return new()
                {
                    new()
                    {
                        Name="Mountain View",
                        Type="Scenic"
                    },
                    new()
                    {
                        Name="Cafe Halt",
                        Type="Food"
                    }
                };
        }
    }
}
