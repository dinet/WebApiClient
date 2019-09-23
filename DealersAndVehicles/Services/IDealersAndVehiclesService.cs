using DealersAndVehicles.Models;
using System;
using System.Threading.Tasks;

namespace DealersAndVehicles.Services
{
    public interface IDealersAndVehiclesService
    {
        Task<Tuple<string, Answer>> GenerateAnswer();
        Task<string> PostAnswer(string datasetId, Answer answer);
    }
}
