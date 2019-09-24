using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealersAndVehicles.Models;
using Microsoft.Extensions.Logging;

namespace DealersAndVehicles.Services
{
    public class DealersAndVehiclesService : IDealersAndVehiclesService
    {
        private readonly IDataRetrievalService _dataRetrievalService;

        private readonly ILogger<DealersAndVehiclesService> _logger;

        public DealersAndVehiclesService(IDataRetrievalService dataRetrievalService, ILogger<DealersAndVehiclesService> logger)
        {
            _dataRetrievalService = dataRetrievalService;
            _logger = logger;
        }

        public async Task<DatasetAnswer> GenerateAnswerAsync()
        {
            try
            {
                //Retrive the dataset Id
                string datasetId = await _dataRetrievalService.RetriveDataSetIdAsync();

                //Retrive vehicle Ids list
                List<int> vehicleIds = await _dataRetrievalService.RetrieveVehicleIdsAsync(datasetId);

                //Retrive vehicles for each vehicleId
                VehicleResponse[] vehicles = await _dataRetrievalService.RetriveVehicleDetailsAsync(datasetId, vehicleIds);

                //Generate Dealer Answer DTO
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

                return new DatasetAnswer() { DataSetId = datasetId, Answer = answer };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw new Exception("An internal error Occured");
            }
        }

        public async Task<string> PostAnswerAsync(string datasetId, Answer answer)
        {
            try
            {
                return await _dataRetrievalService.PostAnswerAsync(datasetId, answer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw new Exception("An internal error Occured");
            }
        }
    }
}
