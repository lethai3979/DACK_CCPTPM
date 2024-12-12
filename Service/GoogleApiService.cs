using Azure;
using GoWheels_WebAPI.Models.GoogleRespone;
using GoWheels_WebAPI.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace GoWheels_WebAPI.Service
{
    public class GoogleApiService : ILocatorService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public GoogleApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["GoogleApi:ApiKey"]!;
        }

        public async Task<DistanceMatrixRespone> GetDistanceAsync(List<(string userId, string location)> userLocations, string  destination)
        {
            var startLocation = string.Join("|", userLocations.Select(loc => loc.location));
            var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={startLocation}&destinations={destination}&key={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            var responeContent = await response.Content.ReadAsStringAsync();
            if(responeContent.IsNullOrEmpty())
            {
                throw new NullReferenceException("Google respone is null");
            }    
            var distanceMatrixRespone = JsonSerializer.Deserialize<DistanceMatrixRespone>(responeContent);
            if(distanceMatrixRespone == null)
            {
                throw new NullReferenceException("Distance matrix respone is null");
            }    
            return distanceMatrixRespone;
        }

        public async Task<DistanceMatrixRespone> GetDistanceAsync(List<(int bookingId, string location)> bookingLocations, string destination)
        {
            var startLocation = string.Join("|", bookingLocations.Select(loc => loc.location));
            var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={startLocation}&destinations={destination}&key={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            var responeContent = await response.Content.ReadAsStringAsync();
            if (responeContent.IsNullOrEmpty())
            {
                throw new NullReferenceException("Google respone is null");
            }
            var distanceMatrixRespone = JsonSerializer.Deserialize<DistanceMatrixRespone>(responeContent);
            if (distanceMatrixRespone == null)
            {
                throw new NullReferenceException("Distance matrix respone is null");
            }
            return distanceMatrixRespone;
        }
    }
}
