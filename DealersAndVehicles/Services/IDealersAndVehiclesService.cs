using DealersAndVehicles.Models;
using System;
using System.Threading.Tasks;

namespace DealersAndVehicles.Services
{
    public interface IDealersAndVehiclesService
    {
        Task<DatasetAnswer> GenerateAnswerAsync();
        Task<string> PostAnswerAsync(string datasetId, Answer answer);
    }
}
