using Contracts.Requests.Route;
using Contracts.Requests.Travel;
using Contracts.Responses.Route;
using Contracts.Responses.Travel;

using Domain.Services;

using FluentValidation;

using MediatR;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediators.Travel
{
    public class Recommendations
    {
        public class Command : RouteRequest, IRequest<List<StopResponse>>
        {
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Destination).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, List<StopResponse>>
        {
            private readonly IRecommendationService _service;

            public Handler(IRecommendationService service)
            {
                _service = service;
            }

            public async Task<List<StopResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                //var validationResult = new CommandValidator().Validate(request);
                //if (!validationResult.IsValid) throw new ValidationException($"{validationResult}");

                return await _service.GetStops(request);
            }
        }
    }
}