using Contracts.Responses.Weather;
using Domain.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mediators.Weather
{
    public class GetWeather
    {
        public class Command : IRequest<WeatherResponse>
        {
            public string City { get; set; }
        }

        public class Handler : IRequestHandler<Command, WeatherResponse>
        {
            private readonly IAccuWeatherService _weather;

            public Handler(IAccuWeatherService weather)
            {
                _weather = weather;
            }

            public async Task<WeatherResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _weather.GetCurrentWeatherAsync(request.City);
            }
        }
    }
}
