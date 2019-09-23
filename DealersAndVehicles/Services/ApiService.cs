using DealersAndVehicles.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace DealersAndVehicles.Services
{
    public class ApiService : IApiService
    {
        private readonly string _baseUrl;
        private readonly HttpClient _httpclient;

        public ApiService(IHttpClientFactory clientFactory)
        {
            _baseUrl = ConfigurationManager.AppSetting["baseUrl"];
            _httpclient = clientFactory.CreateClient();
        }

        public async Task<string> GetDatasetIdAsync()
        {
            try
            {
                var response = await _httpclient.GetAsync($"{_baseUrl}/datasetId");
                DatasetResponse dto = await response.Content.ReadAsAsync<DatasetResponse>();
                return dto.datasetId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<int>> GetVehiclesListAsync(string datasetId)
        {
            try
            {
                var response = await _httpclient.GetAsync($"{_baseUrl}/{datasetId}/vehicles");
                VehicleIdsResponse vehicles = await response.Content.ReadAsAsync<VehicleIdsResponse>();
                return vehicles.vehicleIds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<VehicleResponse> GetVehicleByIdAsync(string datasetId, int vehicleId)
        {
            try
            {
                var response = await _httpclient.GetAsync($"{_baseUrl}/{datasetId}/vehicles/{vehicleId}");
                VehicleResponse dto = await response.Content.ReadAsAsync<VehicleResponse>();
                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DealerResponse> GetDealerByIdAsync(string datasetId, int dealerId)
        {
            try
            {
                var response = await _httpclient.GetAsync($"{_baseUrl}/{datasetId}/dealers/{dealerId}");
                DealerResponse dto = await response.Content.ReadAsAsync<DealerResponse>();
                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> PostAnswerAsync(string datasetId, Answer answer)
        {
            try
            {
                var response = await _httpclient.PostAsJsonAsync($"{_baseUrl}/{datasetId}/answer", answer);
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
