using DealersAndVehicles.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealersAndVehicles.Services
{
    public interface IApiService
    {
        Task<string> GetDatasetIdAsync();
        Task<List<int>> GetVehiclesListAsync(string datasetId);
        Task<VehicleDTO> GetVehicleByIdAsync(string datasetId, int vehicleId);
        Task<DealerDTO> GetDealerByIdAsync(string datasetId, int dealerId);
        Task<string> PostAnswerAsync(string datasetId, AnswerDTO answer);
    }
}
