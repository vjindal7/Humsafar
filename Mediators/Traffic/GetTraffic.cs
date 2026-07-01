using Contracts.Requests.Traffic;
using Contracts.Responses.Traffic;
using Domain.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mediators.Traffic
{
    public class GetTraffic
    {
        public class Command : TrafficRequest, IRequest<TrafficResponse>
        {
        }

        public class Handler : IRequestHandler<Command, TrafficResponse>
        {
            private readonly ITrafficService _trafficService;

            public Handler(ITrafficService trafficService)
            {
                _trafficService = trafficService;
            }

            public async Task<TrafficResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _trafficService.GetTrafficSummaryAsync(
                    request.SourceLat,
                    request.SourceLng,
                    request.DestinationLat,
                    request.DestinationLng,
                    request.TravelMode);
            }
        }
    }
}
