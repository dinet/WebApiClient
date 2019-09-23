using DealersAndVehicles.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealersAndVehicles.Services
{
    public class DataRetrievalService : IDataRetrievalService
    {
        private readonly IApiService _apiService;

        public DataRetrievalService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<string> RetriveDataSetIdAsync()
        {
            return await _apiService.GetDatasetIdAsync();
        }

        public async Task<List<int>> RetrieveVehicleIdsAsync(string datasetId)
        {
            List<int> vehicleIds = new List<int>();
            if (!string.IsNullOrEmpty(datasetId))
            {
                vehicleIds = await _apiService.GetVehiclesListAsync(datasetId);
            }
            return vehicleIds;
        }

        public async Task<VehicleResponse[]> RetriveVehicleDetailsAsync(string datasetId, List<int> vehicleIds)
        {
            if (vehicleIds.Count > 0)
            {
                IEnumerable<Task<VehicleResponse>> vehicleRetrivalTasksQuery = from id in vehicleIds select _apiService.GetVehicleByIdAsync(datasetId, id);
                Task<VehicleResponse>[] vehicleRetrivalTasksArray = vehicleRetrivalTasksQuery.ToArray();
                VehicleResponse[] vehicles = await Task.WhenAll(vehicleRetrivalTasksArray);
                return vehicles;
            }
            else
                return new VehicleResponse[0];
        }

        public List<DealerAnswer> GenerateDealerAnswerDTO(VehicleResponse[] vehicles)
        {
            List<DealerAnswer> dealers = new List<DealerAnswer>();
            if (vehicles.Count() > 0)
            {
                dealers = vehicles.GroupBy(i => i.dealerId).Select(r => new DealerAnswer()
                {
                    dealerId = r.Key,
                    vehicles = r.Select(y => new VehicleAnswer()
                    {
                        make = y.make,
                        model = y.model,
                        vehicleId = y.vehicleId,
                        year = y.year
                    }).ToList()
                }).ToList();
            }
            return dealers;
        }

        public async Task<List<DealerResponse>> RetriveDealerDetailsAsyc(string datasetId, List<int> dealerIds)
        {
            IEnumerable<Task<DealerResponse>> dealerRetrivalTasksQuery = from id in dealerIds select _apiService.GetDealerByIdAsync(datasetId, id);
            Task<DealerResponse>[] dealerRetrivalTasksArray = dealerRetrivalTasksQuery.ToArray();
            DealerResponse[] dealers = await Task.WhenAll(dealerRetrivalTasksArray);
            return dealers.ToList();
        }

        public async Task<string> PostAnswerAsync(string datasetId, Answer answer)
        {
            return await _apiService.PostAnswerAsync(datasetId, answer);
        }
    }
}
