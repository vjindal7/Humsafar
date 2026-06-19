using Contracts.Requests.Travel;
using Contracts.Responses.Travel;
using Domain.Services;

using FluentValidation;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Mediators.Travel
{
    public class Attractions
    {
        public class Command : AttractionRequest, IRequest<AttractionResponse>
        {
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
                RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
                RuleFor(x => x.Mood).NotEmpty();
                RuleFor(x => x.Preferences).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, AttractionResponse>
        {
            private readonly IAttractionService _service;

            public Handler(IAttractionService service)
            {
                _service = service;
            }

            public async Task<AttractionResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = new CommandValidator().Validate(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException($"{validationResult}");
                }

                var prompt = $"""
                    Find nearby attractions within 25 km radius of:

                    Latitude: {request.Latitude}
                    Longitude: {request.Longitude}
                    Mood: {request.Mood}
                    Preferences: {string.Join(", ", request.Preferences ?? new())}

                    Include restaurants, tourist attractions, petrol pumps, shops, cafes, and parks.
                    """;

                return await _service.GetAttractionsAsync(
                    request.Latitude,
                    request.Longitude,
                    request.Mood,
                    request.Preferences);
            }
        }
    }
}
