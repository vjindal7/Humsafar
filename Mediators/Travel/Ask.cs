using Domain.Services;

using FluentValidation;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

using View.Models;

namespace Mediators.Travel
{
    public class Ask
    {
        public class Command : TravelRequest, IRequest<TravelResponse> { }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Message).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, TravelResponse>
        {
            private readonly ITravelAssistantService _service;

            public Handler(
                ITravelAssistantService service)
            {
                _service = service;
            }

            public async Task<TravelResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = new CommandValidator().Validate(request);
                if (!validationResult.IsValid) throw new ValidationException($"{validationResult}");

                var message = await _service.GetTravelResponseAsync(request.Message);
                return new TravelResponse() { Message = message };
            }
        }
    }
}