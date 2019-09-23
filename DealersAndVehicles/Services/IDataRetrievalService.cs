using System.Collections.Generic;
using System.Threading.Tasks;
using DealersAndVehicles.Models;

namespace DealersAndVehicles.Services
{
    public interface IDataRetrievalService
    {
        List<DealerAnswer> GroupVehiclesBydealerId(VehicleResponse[] vehicles);
        Task<List<int>> RetrieveVehicleIdsAsync(string datasetId);
        Task<List<DealerResponse>> RetriveDealerDetailsAsyc(string datasetId, List<int> dealerIds);
        Task<VehicleResponse[]> RetriveVehicleDetailsAsync(string datasetId, List<int> vehicleIds);
        Task<string> PostAnswerAsync(string datasetId, Answer answer);
        Task<string> RetriveDataSetId();
    }
}