using DealersAndVehicles.Models;
using DealersAndVehicles.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DealersAndVehicles.Tests
{
    public class DealersAndVehiclesServiceTests
    {
        private readonly IDealersAndVehiclesService _dealersAndVehiclesService;
        public DealersAndVehiclesServiceTests()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                  .AddHttpClient()
                  .AddSingleton<IApiService, ApiService>()
                  .AddSingleton<IDataRetrievalService, DataRetrievalService>()
                  .AddSingleton<IDealersAndVehiclesService, DealersAndVehiclesService>()
                  .BuildServiceProvider();
            _dealersAndVehiclesService = serviceProvider.GetService<IDealersAndVehiclesService>();
        }

        [Test]
        public async Task GenerateAnswerAsync_AnswerShouldMatchTheCheatAPIAnswer()
        {
            HttpClient client = new HttpClient();
            DatasetAnswer answer = await _dealersAndVehiclesService.GenerateAnswerAsync();
            var response = client.GetAsync($"http://api.coxauto-interview.com/api/{answer.DataSetId}/cheat").Result;
            Answer expectedResult = await response.Content.ReadAsAsync<Answer>();
            Assert.IsTrue(expectedResult.dealers.OrderBy(i => i.dealerId).Select(i => i.dealerId)
                .SequenceEqual(answer.Answer.dealers.OrderBy(i => i.dealerId).Select(i => i.dealerId)));
        }
    }
}