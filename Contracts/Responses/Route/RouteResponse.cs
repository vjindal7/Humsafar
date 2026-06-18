using System.Collections.Generic;

namespace Contracts.Responses.Route
{
    public class RouteResponse
    {
        public string Source { get; set; }

        public string Destination { get; set; }

        public string Distance { get; set; }

        public string Duration { get; set; }

        public List<string> Stops { get; set; }
    }
}
