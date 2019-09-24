using DealersAndVehicles.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;

namespace DealersAndVehicles.Services
{
    public class ApiService : IApiService
    {
        private readonly string _baseUrl;
        private readonly HttpClient _httpclient;

        public ApiService(IHttpClientFactory clientFactory, ILogger<ApiService> logger)
        {
            _baseUrl = ConfigurationManager.AppSetting["baseUrl"];
            _httpclient = clientFactory.CreateClient();
        }

        public async Task<string> GetDatasetIdAsync()
        {
            var response = await _httpclient.GetAsync($"{_baseUrl}/datasetId");
            DatasetResponse dto = await response.Content.ReadAsAsync<DatasetResponse>();
            return dto.datasetId;
        }

        public async Task<List<int>> GetVehiclesListAsync(string datasetId)
        {
            var response = await _httpclient.GetAsync($"{_baseUrl}/{datasetId}/vehicles");
            VehicleIdsResponse vehicles = await response.Content.ReadAsAsync<VehicleIdsResponse>();
            return vehicles.vehicleIds;
        }

        public async Task<VehicleResponse> GetVehicleByIdAsync(string datasetId, int vehicleId)
        {
            var response = await _httpclient.GetAsync($"{_baseUrl}/{datasetId}/vehicles/{vehicleId}");
            VehicleResponse dto = await response.Content.ReadAsAsync<VehicleResponse>();
            return dto;
        }

        public async Task<DealerResponse> GetDealerByIdAsync(string datasetId, int dealerId)
        {
            var response = await _httpclient.GetAsync($"{_baseUrl}/{datasetId}/dealers/{dealerId}");
            DealerResponse dto = await response.Content.ReadAsAsync<DealerResponse>();
            return dto;
        }

        public async Task<string> PostAnswerAsync(string datasetId, Answer answer)
        {
            var response = await _httpclient.PostAsJsonAsync($"{_baseUrl}/{datasetId}/answer", answer);
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
