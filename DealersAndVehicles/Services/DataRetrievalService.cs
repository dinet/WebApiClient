using DealersAndVehicles.Models;
using Microsoft.Extensions.Logging;
using System;
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

        //Retrives dataset ID
        public async Task<string> RetriveDataSetIdAsync()
        {
            return await _apiService.GetDatasetIdAsync();
        }

        //Retrives vehicles Ids by dataset id
        public async Task<List<int>> RetrieveVehicleIdsAsync(string datasetId)
        {
            return await _apiService.GetVehiclesListAsync(datasetId);
        }

        //Retrives vehicle details by vehicles ids
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

        //Generates dealer answer DTO
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

        //Retrives dealer details
        public async Task<List<DealerResponse>> RetriveDealerDetailsAsyc(string datasetId, List<int> dealerIds)
        {
            IEnumerable<Task<DealerResponse>> dealerRetrivalTasksQuery = from id in dealerIds select _apiService.GetDealerByIdAsync(datasetId, id);
            Task<DealerResponse>[] dealerRetrivalTasksArray = dealerRetrivalTasksQuery.ToArray();
            DealerResponse[] dealers = await Task.WhenAll(dealerRetrivalTasksArray);
            return dealers.ToList();
        }

        //Posts answer
        public async Task<string> PostAnswerAsync(string datasetId, Answer answer)
        {
            return await _apiService.PostAnswerAsync(datasetId, answer);
        }
    }
}
