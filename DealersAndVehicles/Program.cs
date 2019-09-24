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
                     .AddSingleton<IDataRetrievalService, DataRetrievalService>()
                     .BuildServiceProvider();

            IDealersAndVehiclesService dealersAndVehiclesService = serviceProvider.GetService<IDealersAndVehiclesService>();
            Task<DatasetAnswer> answer = dealersAndVehiclesService.GenerateAnswerAsync();
            Task<string> response = dealersAndVehiclesService.PostAnswerAsync(answer.Result.DataSetId, answer.Result.Answer);

            Console.WriteLine(response.Result);
            Console.ReadLine();
        }
    }
}
