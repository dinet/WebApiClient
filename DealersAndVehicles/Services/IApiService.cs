using DealersAndVehicles.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealersAndVehicles.Services
{
    public interface IApiService
    {
        Task<string> GetDatasetIdAsync();
        Task<List<int>> GetVehiclesListAsync(string datasetId);
        Task<VehicleResponse> GetVehicleByIdAsync(string datasetId, int vehicleId);
        Task<DealerResponse> GetDealerByIdAsync(string datasetId, int dealerId);
        Task<string> PostAnswerAsync(string datasetId, Answer answer);
    }
}
