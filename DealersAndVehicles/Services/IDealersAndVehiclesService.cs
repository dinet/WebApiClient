using System.Threading.Tasks;

namespace DealersAndVehicles.Services
{
    public interface IDealersAndVehiclesService
    {
        Task<string> RetriveAndPostRecords();
    }
}
