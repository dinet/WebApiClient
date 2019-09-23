using System;
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

        public async Task<Tuple<string, Answer>> GenerateAnswer()
        {
            //Retrive the dataset Id
            string datasetId = await _apiService.GetDatasetIdAsync();

            //Retrive vehicle Ids list
            List<int> vehicleIds = await RetrieveVehicleIdsAsync(datasetId);

            //Retrive vehicles for each vehicleId
            VehicleResponse[] vehicles = await RetriveVehicleDetailsAsync(datasetId, vehicleIds);

            //Group vehicles by dealerIds
            List<DealerAnswer> dealerAnswers = GroupVehiclesBydealerId(vehicles);

            //Get distinct dealerIds list
            List<int> dealerIds = dealerAnswers.Select(i => i.dealerId).ToList();

            //Retrive dealers by dealerId
            List<DealerResponse> dealerReponses = await RetriveDealerDetailsAsyc(datasetId, dealerIds);

            //Setting dealer names in dealer answer
            dealerAnswers.ForEach(i => i.name = dealerReponses.FirstOrDefault(d => d.dealerId == i.dealerId)?.name);

            //Preparing the Answer
            Answer answer = new Answer()
            {
                dealers = dealerAnswers.ToList()
            };

            return new Tuple<string, Answer>(datasetId, answer);
        }

        public async Task<string> PostAnswer(string datasetId, Answer answer)
        {
            return await _apiService.PostAnswerAsync(datasetId, answer);
        }

        private async Task<List<int>> RetrieveVehicleIdsAsync(string datasetId)
        {
            List<int> vehicleIds = new List<int>();
            if (!string.IsNullOrEmpty(datasetId))
            {
                vehicleIds = await _apiService.GetVehiclesListAsync(datasetId);
            }
            return vehicleIds;
        }

        private async Task<VehicleResponse[]> RetriveVehicleDetailsAsync(string datasetId, List<int> vehicleIds)
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

        private List<DealerAnswer> GroupVehiclesBydealerId(VehicleResponse[] vehicles)
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

        private async Task<List<DealerResponse>> RetriveDealerDetailsAsyc(string datasetId, List<int> dealerIds)
        {
            IEnumerable<Task<DealerResponse>> dealerRetrivalTasksQuery = from id in dealerIds select _apiService.GetDealerByIdAsync(datasetId, id);
            Task<DealerResponse>[] dealerRetrivalTasksArray = dealerRetrivalTasksQuery.ToArray();
            DealerResponse[] dealers = await Task.WhenAll(dealerRetrivalTasksArray);
            return dealers.ToList();
        }
    }
}
