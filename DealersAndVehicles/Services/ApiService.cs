using DealersAndVehicles.DTO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DealersAndVehicles.Services
{
    public class ApiService : IApiService
    {
        private readonly string _baseUrl;
        private readonly IHttpClientFactory _clientFactory;

        public ApiService(IHttpClientFactory clientFactory)
        {
            _baseUrl = ConfigurationManager.AppSetting["baseUrl"];
            _clientFactory = clientFactory;
        }

        public async Task<string> GetDatasetIdAsync()
        {
            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{_baseUrl}/datasetId");
            DatasetDTO dto = await response.Content.ReadAsAsync<DatasetDTO>();
            return dto.datasetId;
        }

        public async Task<List<int>> GetVehiclesListAsync(string datasetId)
        {
            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{_baseUrl}/{datasetId}/vehicles");
            VehicleIdsDTO vehicles = await response.Content.ReadAsAsync<VehicleIdsDTO>();
            return vehicles.vehicleIds;
        }

        public async Task<VehicleDTO> GetVehicleByIdAsync(string datasetId, int vehicleId)
        {
            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{_baseUrl}/{datasetId}/vehicles/{vehicleId}");
            VehicleDTO dto = await response.Content.ReadAsAsync<VehicleDTO>();
            return dto;
        }

        public async Task<DealerDTO> GetDealerByIdAsync(string datasetId, int dealerId)
        {
            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{_baseUrl}/{datasetId}/dealers/{dealerId}");
            DealerDTO dto = await response.Content.ReadAsAsync<DealerDTO>();
            return dto;
        }

        public async Task<string> PostAnswerAsync(string datasetId, AnswerDTO answer)
        {
            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync($"{_baseUrl}/{datasetId}/answer", answer);
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
