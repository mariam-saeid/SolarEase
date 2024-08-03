using Newtonsoft.Json.Linq;
using SolarEase.Models.DTO.SolarInstallerDto;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolarEase.Services
{
    public class GoogleMapsApiService
    {
        public async Task<GeoLocation> GetGeoLocation(HttpClient _httpClient, string address, string city)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={address},{city}&key=");

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(json);

                var location = jObject["results"]?[0]?["geometry"]?["location"];
                if (location == null)
                {
                    throw new Exception("Invalid response structure: location information not found.");
                }

                var latitude = location["lat"]?.Value<double>() ?? throw new Exception("Latitude not found.");
                var longitude = location["lng"]?.Value<double>() ?? throw new Exception("Longitude not found.");

                return new GeoLocation { Latitude = latitude, Longitude = longitude };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting location from Google Maps API: {ex.Message}");
                throw;
            }
        }

        public async Task<double> GetDistance(HttpClient _httpClient, double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://maps.googleapis.com/maps/api/distancematrix/json?origins={originLatitude},{originLongitude}&destinations={destinationLatitude},{destinationLongitude}&key=");

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(json);

                var distanceToken = jObject["rows"]?[0]?["elements"]?[0]?["distance"]?["value"];
                if (distanceToken == null)
                {
                    throw new Exception("Invalid response structure: distance information not found.");
                }

                var distance = distanceToken.Value<double>();

                // Convert meters to kilometers
                return distance / 1000;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting distance from Google Maps API: {ex.Message}");
                return -1; // Return -1 in case of an error
            }
        }
    }
}
