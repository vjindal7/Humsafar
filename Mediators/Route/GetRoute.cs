using Contracts.Responses.Route;
using Domain.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mediators.Route
{
    public class GetRoute
    {
        public class Command : IRequest<RouteResponse>
        {
            public string Source { get; set; }
            public string Destination { get; set; }
        }

        public class Handler : IRequestHandler<Command, RouteResponse>
        {
            private readonly IRouteService _service;

            public Handler(IRouteService service)
            {
                _service = service;
            }

            public async Task<RouteResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _service.GenerateAsync(request.Source, request.Destination);
            }
        }
    }
}
