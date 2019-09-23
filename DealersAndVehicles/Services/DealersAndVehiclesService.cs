using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealersAndVehicles.Models;

namespace DealersAndVehicles.Services
{
    public class DealersAndVehiclesService : IDealersAndVehiclesService
    {
        private readonly IDataRetrievalService _dataRetrievalService;

        public DealersAndVehiclesService(IDataRetrievalService dataRetrievalService)
        {
            _dataRetrievalService = dataRetrievalService;
        }

        public async Task<Tuple<string, Answer>> GenerateAnswer()
        {
            //Retrive the dataset Id
            string datasetId = await _dataRetrievalService.RetriveDataSetIdAsync();

            //Retrive vehicle Ids list
            List<int> vehicleIds = await _dataRetrievalService.RetrieveVehicleIdsAsync(datasetId);

            //Retrive vehicles for each vehicleId
            VehicleResponse[] vehicles = await _dataRetrievalService.RetriveVehicleDetailsAsync(datasetId, vehicleIds);

            //Group vehicles by dealerIds
            List<DealerAnswer> dealerAnswers = _dataRetrievalService.GenerateDealerAnswerDTO(vehicles);

            //Get distinct dealerIds list
            List<int> dealerIds = dealerAnswers.Select(i => i.dealerId).ToList();

            //Retrive dealers by dealerId
            List<DealerResponse> dealerReponses = await _dataRetrievalService.RetriveDealerDetailsAsyc(datasetId, dealerIds);

            //Setting dealer names in dealer answer
            dealerAnswers.ForEach(i => i.name = dealerReponses.FirstOrDefault(d => d.dealerId == i.dealerId)?.name);

            //Preparing the Answer
            Answer answer = new Answer()
            {
                dealers = dealerAnswers.ToList()
            };

            return new Tuple<string, Answer>(datasetId, answer);
        }

        public async Task<string> PostAnswerAsync(string datasetId, Answer answer)
        {
            return await _dataRetrievalService.PostAnswerAsync(datasetId, answer);
        }
    }
}
