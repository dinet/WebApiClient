using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DealersAndVehicles.Services;
using System;
using DealersAndVehicles.Models;

namespace DealersAndVehicles
{
    class Program
    {
        private static IDealersAndVehiclesService dealersAndVehiclesService;
        static void Main(string[] args)
        {
            ConfigureServices();

            //Generating Answer
            Task<DatasetAnswer> answer = dealersAndVehiclesService.GenerateAnswerAsync();
            //Posting Answer
            Task<string> response = dealersAndVehiclesService.PostAnswerAsync(answer.Result.DataSetId, answer.Result.Answer);

            Console.WriteLine(response.Result);
            Console.ReadLine();
        }

        private static void ConfigureServices()
        {
            //Configuring services
            ServiceProvider serviceProvider = new ServiceCollection()
                     .AddHttpClient()
                     .AddSingleton<IApiService, ApiService>()
                     .AddSingleton<IDealersAndVehiclesService, DealersAndVehiclesService>()
                     .AddSingleton<IDataRetrievalService, DataRetrievalService>()
                     .BuildServiceProvider();
            dealersAndVehiclesService = serviceProvider.GetService<IDealersAndVehiclesService>();
        }
    }
}
