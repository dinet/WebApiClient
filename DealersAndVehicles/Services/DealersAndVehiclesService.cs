using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealersAndVehicles.DTO;

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
            IEnumerable<Task<VehicleDTO>> getVehicleTasksQuery = from id in vehicleIds select _apiService.GetVehicleByIdAsync(datasetId, id);
            Task<VehicleDTO>[] getVehicleTasksArr = getVehicleTasksQuery.ToArray();
            VehicleDTO[] vehicles = await Task.WhenAll(getVehicleTasksArr);

            //Grouping vehicles by dealer Id
            var vehiclesGroupedByDealers = vehicles.GroupBy(i => i.dealerId);

            //Retriving Dealers and preparing dealer DTOs for the answer 
            IEnumerable<Task<DealerAnswerDTO>> getDealerTasksQuery = from g in vehiclesGroupedByDealers select PrepareDealerDTOs(datasetId, g.Key, g.ToList());
            Task<DealerAnswerDTO>[] getDealerTasksArray = getDealerTasksQuery.ToArray();
            DealerAnswerDTO[] Dealers = await Task.WhenAll(getDealerTasksArray);

            //Preparing the Answer DTO
            AnswerDTO answer = new AnswerDTO()
            {
                dealers = Dealers.ToList()
            };

            //Post Answer
            string response = await _apiService.PostAnswerAsync(datasetId, answer);
            return response;
        }

        private async Task<DealerAnswerDTO> PrepareDealerDTOs(string datasetId, int dealerId, List<VehicleDTO> vehicles)
        {
            //Retrive dealer details
            DealerDTO dealerDTO = await _apiService.GetDealerByIdAsync(datasetId, dealerId);

            //Prepare vehicles list
            List<VehicleAnswerDTO> vehicleAnsDTOs = vehicles.Select(i => new VehicleAnswerDTO()
            {
                vehicleId = i.vehicleId,
                make = i.make,
                model = i.model,
                year = i.year
            }).ToList();

            //Prepare dealers list
            DealerAnswerDTO dealer = new DealerAnswerDTO()
            {
                dealerId = dealerDTO.dealerId,
                name = dealerDTO.name,
                vehicles = vehicleAnsDTOs
            };
            return dealer;
        }
    }
}
