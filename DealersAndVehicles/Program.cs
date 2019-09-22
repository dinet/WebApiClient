using DealersAndVehicles.DTO;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
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
            var serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .AddSingleton<IApiService, ApiService>()
            .AddSingleton<IDealersAndVehiclesService, DealersAndVehiclesService>()
            .BuildServiceProvider();

            var dealersAndVehiclesService = serviceProvider.GetService<IDealersAndVehiclesService>();

            var response = dealersAndVehiclesService.RetriveAndPostRecords();

            Console.WriteLine(response.Result);
            Console.ReadLine();
            //using (var httpClient = new HttpClient())
            //{
            //    Stopwatch stopWatch = new Stopwatch();
            //    stopWatch.Start();
            //    try
            //    {
            //        var result = SomeAsyncCode().GetAwaiter().GetResult();
            //    }
            //    catch (ArgumentException aex)
            //    {
            //        Console.WriteLine($"Caught ArgumentException: {aex.Message}");
            //    }
            //    stopWatch.Stop();
            //    TimeSpan ts = stopWatch.Elapsed;
            //    Console.WriteLine(ts.Seconds);
            //}
        }

        private static async Task<string> SomeAsyncCode()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("http://api.coxauto-interview.com/api/datasetId");
                DatasetDTO dto = await response.Content.ReadAsAsync<DatasetDTO>();

                var rasdp = await httpClient.GetAsync($"http://api.coxauto-interview.com/api/{dto.datasetId}/vehicles");
                VehicleIdsDTO vehicles = await rasdp.Content.ReadAsAsync<VehicleIdsDTO>();

                var downloadTasksQuery = from id in vehicles.vehicleIds select ProcessURLAsync($"http://api.coxauto-interview.com/api/{dto.datasetId}/vehicles/{id}", httpClient);
                var downloadTasks = downloadTasksQuery.ToArray();
                var resultasdfs = await Task.WhenAll(downloadTasks);

                var groupedDealers = resultasdfs.GroupBy(i => i.dealerId);

                var downloadTasksQuerys = from dealer in groupedDealers select ProcessURLAsyncDealers($"http://api.coxauto-interview.com/api/{dto.datasetId}/dealers/{dealer.Key}", httpClient, dealer.ToList());
                var downloadTaskss = downloadTasksQuerys.ToArray();
                var resultasdfss = await Task.WhenAll(downloadTaskss);

                AnswerDTO answer = new AnswerDTO()
                {
                    dealers = resultasdfss.ToList()
                };

                var asdfasdfdas = await httpClient.PostAsJsonAsync($"http://api.coxauto-interview.com/api/{dto.datasetId}/answer", answer);
                string content = await asdfasdfdas.Content.ReadAsStringAsync();
                return content;
            }
        }

        private static async Task<VehicleDTO> ProcessURLAsync(string url, HttpClient client)
        {
            var rpii = await client.GetAsync(url);
            var vehicle = await rpii.Content.ReadAsAsync<VehicleDTO>();
            return vehicle;
        }

        private static async Task<DealerAnswerDTO> ProcessURLAsyncDealers(string url, HttpClient client, List<VehicleDTO> vehicles)
        {
            var rpii = await client.GetAsync(url);
            var dealerdt = await rpii.Content.ReadAsAsync<DealerAnswerDTO>();
            List<VehicleAnswerDTO> vehcs = vehicles.Select(i => new VehicleAnswerDTO()
            {
                vehicleId = i.vehicleId,
                make = i.make,
                model = i.model,
                year = i.year
            }).ToList();
            DealerAnswerDTO dealer = new DealerAnswerDTO()
            {
                dealerId = dealerdt.dealerId,
                name = dealerdt.name,
                vehicles = vehcs
            };
            return dealer;
        }
    }
}
