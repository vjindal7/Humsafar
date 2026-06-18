using Contracts.Requests.Travel;
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
    public class Ask
    {
        public class Command : TravelRequest, IRequest<TravelResponse>
        {
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Destination).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, TravelResponse>
        {
            private readonly ITravelAssistantService _service;

            public Handler(ITravelAssistantService service)
            {
                _service = service;
            }

            public async Task<TravelResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = new CommandValidator().Validate(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(
                        $"{validationResult}");
                }

                var prompt = $"""
                        Plan a trip with:

                        Destination: {request.Destination}
                        Trip Type: {request.TripType}
                        Companion: {request.CompanionType}
                        Mood: {request.Mood}
                        Budget: {request.Budget}
                        """;

                var summary = await _service.GetTravelResponseAsync(prompt);

                return new TravelResponse
                {
                    TripId = Guid.NewGuid(),
                    JourneySummary = summary,
                    Suggestions = new List<string>
                        {
                            "Check weather",
                            "Plan stops",
                            "Carry essentials"
                        }
                };
            }
        }
    }
}