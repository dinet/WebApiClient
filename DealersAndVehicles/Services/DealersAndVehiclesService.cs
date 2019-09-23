using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealersAndVehicles.Models;

namespace DealersAndVehicles.Services
{
    public class DealersAndVehiclesService : IDealersAndVehiclesService
    {
        private readonly IApiService _apiService;
        public DealersAndVehiclesService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<string> RetriveAndPostRecords()
        {
            //Retriving the dataset Id
            string datasetId = await _apiService.GetDatasetIdAsync();

            //Retriving the vehicle Ids list
            List<int> vehicleIds = await _apiService.GetVehiclesListAsync(datasetId);

            //Retriving vehicles for each vehicleId
            IEnumerable<Task<VehicleResponse>> getVehicleTasksQuery = from id in vehicleIds select _apiService.GetVehicleByIdAsync(datasetId, id);
            Task<VehicleResponse>[] getVehicleTasksArr = getVehicleTasksQuery.ToArray();
            VehicleResponse[] vehicles = await Task.WhenAll(getVehicleTasksArr);

            //Grouping vehicles by dealer Id
            var vehiclesGroupedByDealers = vehicles.GroupBy(i => i.dealerId);

            //Retriving Dealers and preparing dealer DTOs for the answer 
            IEnumerable<Task<DealerAnswer>> getDealerTasksQuery = from g in vehiclesGroupedByDealers select PrepareDealerDTOs(datasetId, g.Key, g.ToList());
            Task<DealerAnswer>[] getDealerTasksArray = getDealerTasksQuery.ToArray();
            DealerAnswer[] dealers = await Task.WhenAll(getDealerTasksArray);

            //Preparing the Answer DTO
            Answer answer = new Answer()
            {
                dealers = dealers.ToList()
            };

            //Post Answer
            string response = await _apiService.PostAnswerAsync(datasetId, answer);
            return response;
        }

        private async Task<DealerAnswer> PrepareDealerDTOs(string datasetId, int dealerId, List<VehicleResponse> vehicles)
        {
            //Retrive dealer details
            DealerResponse dealerDTO = await _apiService.GetDealerByIdAsync(datasetId, dealerId);

            //Prepare vehicles list
            List<VehicleAnswer> vehicleAnsDTOs = vehicles.Select(i => new VehicleAnswer()
            {
                vehicleId = i.vehicleId,
                make = i.make,
                model = i.model,
                year = i.year
            }).ToList();

            //Prepare dealers list
            DealerAnswer dealer = new DealerAnswer()
            {
                dealerId = dealerDTO.dealerId,
                name = dealerDTO.name,
                vehicles = vehicleAnsDTOs
            };
            return dealer;
        }
    }
}
