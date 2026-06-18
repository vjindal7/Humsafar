namespace Contracts.Responses.Weather
{
    public class WeatherResponse
    {
        public string City { get; set; }

        public string Condition { get; set; }

        public decimal TemperatureC { get; set; }

        public bool IsTravelRecommended =>
            TemperatureC >= 10 &&
            TemperatureC <= 35;
    }
}
