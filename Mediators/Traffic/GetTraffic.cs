using Contracts.Requests.Traffic;
using Contracts.Responses.Traffic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediators.Traffic
{
    public class GetTraffic
    {
        public class Command : IRequest<TrafficResponse>
        {
            public string Source { get; set; } = string.Empty;
            public string Destination { get; set; } = string.Empty;
        }

        public class Handler : IRequestHandler<Command, TrafficResponse>
        {
            public async Task<TrafficResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                // Returning static data as requested for now
                return await Task.FromResult(new TrafficResponse
                {
                    Routes = new List<TrafficRoute>
                    {
                        new TrafficRoute
                        {
                            Summary = new TrafficSummary
                            {
                                LengthInMeters = 14500,
                                TravelTimeInSeconds = 1800,
                                TrafficDelayInSeconds = 420,
                                ArrivalTime = DateTime.Parse("2026-06-17T15:20:00Z", null, System.Globalization.DateTimeStyles.RoundtripKind)
                            }
                        }
                    }
                });
            }
        }
    }
}
