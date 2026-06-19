using Contracts.Responses.Travel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IAttractionService
    {
        Task<AttractionResponse> GetAttractionsAsync(
            double latitude,
            double longitude,
            string mood,
            List<string> preferences);
    }

    public class AttractionService : IAttractionService
    {
        private static readonly Random _rng = new();

        // Pre-defined dummy places within a ~25 km radius (coordinates offset by ~0.22 deg)
        private static readonly List<(string Name, string Category, string Description, double LatOffset, double LonOffset, double Rating)> _dummyPlaces = new()
        {
            // Restaurants
            ("The Grand Kitchen",   "Restaurant",     "Fine dining with multi-cuisine menu",      -0.05,  0.08,  4.5),
            ("Spice Bazaar",        "Restaurant",     "Authentic local street food experience",    0.10,  -0.03,  4.2),
            ("Green Bowl Cafe",     "Restaurant",     "Healthy salads, smoothies & organic bowls", 0.15,  0.12,  4.0),
            ("Pizza Paradise",      "Restaurant",     "Wood-fired pizzas and pasta",              -0.12, -0.10,  3.8),
            ("Dragon Wok",          "Restaurant",     "Chinese and Thai specialities",             0.02,  0.18,  4.3),

            // Tourist Attractions
            ("Sunset Viewpoint",    "Tourist Attraction", "Scenic hilltop with panoramic sunset views",     0.20,  0.05,  4.8),
            ("Heritage Fort",       "Tourist Attraction", "17th-century fort with guided tours",           -0.18,  0.15,  4.6),
            ("Botanical Gardens",   "Tourist Attraction", "50-acre gardens with exotic plants and trails",  0.08, -0.20,  4.4),
            ("Art Museum",          "Tourist Attraction", "Modern and contemporary art exhibitions",       -0.15, -0.08,  4.1),
            ("Adventure Park",      "Tourist Attraction", "Zip-lining, rock climbing & obstacle courses",   0.22, -0.12,  4.7),

            // Petrol Pumps
            ("Shell Petrol Pump",       "Petrol Pump", "24/7 fuel station with convenience store",    0.03,  0.05,  4.0),
            ("Indian Oil Station",      "Petrol Pump", "Fuel, air & basic vehicle check",             -0.08, -0.15,  3.6),
            ("Bharat Petroleum",        "Petrol Pump", "Car wash and tyre inflation available",        0.12,  0.02,  3.8),

            // Shops
            ("Craft Emporium",          "Shop",        "Handicrafts, souvenirs & local art",          -0.10,  0.10,  4.3),
            ("Tech Mart",               "Shop",        "Electronics, gadgets & accessories",           0.05, -0.18,  4.0),
            ("Book Nook",               "Shop",        "Books, maps & travel guides",                 -0.22,  0.08,  4.2),
            ("Fashion Street",          "Shop",        "Clothing, accessories & footwear",             0.18,  0.15,  3.7),

            // Cafes
            ("Brew & Beans",            "Cafe",        "Speciality coffee and fresh pastries",         0.07,  0.07,  4.4),
            ("Tea Villa",               "Cafe",        "Premium teas and light snacks",               -0.03, -0.05,  4.1),
            ("The Reading Room",        "Cafe",        "Quiet cafe with library and Wi-Fi",            0.15, -0.08,  4.5),

            // Parks & Nature
            ("Lakeside Promenade",      "Park",        "Walking trail around a scenic lake",           0.12,  0.15,  4.6),
            ("City Central Park",       "Park",        "Open lawns, fountains & jogging track",       -0.20, -0.05,  4.3),
        };

        public async Task<AttractionResponse> GetAttractionsAsync(
            double latitude,
            double longitude,
            string mood,
            List<string> preferences)
        {
            await Task.Delay(200); // simulate async work

            var items = _dummyPlaces
                .Select(p => new AttractionItem
                {
                    Name = p.Name,
                    Category = p.Category,
                    Description = p.Description,
                    Latitude = latitude + p.LatOffset,
                    Longitude = longitude + p.LonOffset,
                    DistanceKm = Math.Round(CalculateDistance(latitude, longitude, latitude + p.LatOffset, longitude + p.LonOffset), 2),
                    Rating = p.Rating
                })
                .ToList();

            // Filter by preferences if provided
            if (preferences is { Count: > 0 })
            {
                var prefs = preferences.Select(p => p.ToLowerInvariant()).ToList();

                items = items
                    .Where(i => prefs.Contains(i.Category.ToLowerInvariant())
                             || prefs.Any(p => i.Name.Contains(p, StringComparison.OrdinalIgnoreCase))
                             || prefs.Any(p => i.Description.Contains(p, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            // Sort based on mood
            items = SortByMood(items, mood);

            return new AttractionResponse
            {
                Mood = mood,
                Latitude = latitude,
                Longitude = longitude,
                TotalResults = items.Count,
                Attractions = items
            };
        }

        private static List<AttractionItem> SortByMood(List<AttractionItem> items, string mood)
        {
            var moodKey = mood?.ToLowerInvariant() ?? string.Empty;

            // Define a priority order for categories based on mood
            List<string> categoryPriority = moodKey switch
            {
                "hungry"    => new() { "Restaurant", "Cafe", "Shop", "Park", "Tourist Attraction", "Petrol Pump" },
                "energetic" => new() { "Tourist Attraction", "Park", "Restaurant", "Cafe", "Shop", "Petrol Pump" },
                "tired"     => new() { "Cafe", "Restaurant", "Park", "Petrol Pump", "Shop", "Tourist Attraction" },
                "happy"     => new() { "Park", "Tourist Attraction", "Cafe", "Restaurant", "Shop", "Petrol Pump" },
                _           => new() { "Restaurant", "Cafe", "Tourist Attraction", "Park", "Shop", "Petrol Pump" }
            };

            return items
                .OrderBy(i =>
                {
                    var idx = categoryPriority.IndexOf(i.Category);
                    return idx < 0 ? int.MaxValue : idx;
                })
                .ThenByDescending(i => i.Rating)
                .ToList();
        }

        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in km
            var dLat = ToRad(lat2 - lat1);
            var dLon = ToRad(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRad(double deg) => deg * Math.PI / 180;
    }
}
