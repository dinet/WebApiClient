using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DealersAndVehicles.Services;
using System;
using DealersAndVehicles.Models;

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
            Task<Tuple<string, Answer>> answer = dealersAndVehiclesService.GenerateAnswer();
            Task<string> response = dealersAndVehiclesService.PostAnswer(answer.Result.Item1, answer.Result.Item2);

            Console.WriteLine(response.Result);
            Console.ReadLine();
        }
    }
}
