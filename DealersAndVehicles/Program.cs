using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DealersAndVehicles.Services;
using System;

namespace DealersAndVehicles
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .AddSingleton<IApiService, ApiService>()
            .AddSingleton<IDealersAndVehiclesService, DealersAndVehiclesService>()
            .BuildServiceProvider();

            IDealersAndVehiclesService dealersAndVehiclesService = serviceProvider.GetService<IDealersAndVehiclesService>();

            Task<string> response = dealersAndVehiclesService.RetriveAndPostRecords();

            Console.WriteLine(response.Result);
            Console.ReadLine();
        }
    }
}
