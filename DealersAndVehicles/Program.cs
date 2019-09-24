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

            try
            {
                Console.WriteLine("Processing...");
                //Generating Answer
                Task<DatasetAnswer> answer = dealersAndVehiclesService.GenerateAnswerAsync();
                //Posting Answer
                Task<string> response = dealersAndVehiclesService.PostAnswerAsync(answer.Result.DataSetId, answer.Result.Answer);
                Console.Clear();
                Console.WriteLine($"Program finished with the following message\n\n{ response.Result}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
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
