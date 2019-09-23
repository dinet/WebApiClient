using DealersAndVehicles.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealersAndVehicles.Tests
{
    class DataRetrievalServiceTests
    {
        private readonly IDataRetrievalService _dataRetrievalService;
        private readonly string _dataSetId;
        public DataRetrievalServiceTests()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                  .AddHttpClient()
                  .AddSingleton<IApiService, ApiService>()
                  .AddSingleton<IDataRetrievalService, DataRetrievalService>()
                  .BuildServiceProvider();
            _dataRetrievalService = serviceProvider.GetService<IDataRetrievalService>();
            _dataSetId = "5yv1I3dA1wg";
        }

        [Test]
        public async Task RetriveDataSetId_ShouldReturnString()
        {
            var datasetId = await _dataRetrievalService.RetriveDataSetIdAsync();
            Assert.IsInstanceOf(typeof(string), datasetId);
        }

        [Test]
        public async Task RetrieveVehicleIdsAsync_ShouldReturnVehicleListForValidDataSetId()
        {
            var vehicles = await _dataRetrievalService.RetrieveVehicleIdsAsync(_dataSetId);
            Assert.Greater(vehicles.Count, 0);
        }

        [Test]
        public async Task RetrieveVehicleIdsAsync_ShouldReturnEmptyForNullDatasetId()
        {
            var vehicles = await _dataRetrievalService.RetrieveVehicleIdsAsync(null);
            Assert.AreEqual(vehicles.Count, 0);
        }

        [Test]
        public async Task RetrieveVehicleIdsAsync_ShouldReturnEmptyForIncorrectDatasetId()
        {
            var vehicles = await _dataRetrievalService.RetrieveVehicleIdsAsync("qeqwrqwerqw");
            Assert.AreEqual(vehicles.Count, 0);
        }

        [Test]
        public async Task RetriveVehicleDetailsAsync_ShouldReturnListForVechileIds()
        {
            List<int> vehicleIds = new List<int>
            {
                384297551,
                2068036871,
                1781801989
            };

            var vehicles = await _dataRetrievalService.RetriveVehicleDetailsAsync(_dataSetId, vehicleIds);
            Assert.Greater(vehicles.Length, 0);
        }

        [Test]
        public async Task RetriveVehicleDetailsAsync_ShouldReturnEmptyForEmptyVehicleIds()
        {
            List<int> vehicleIds = new List<int>();
            var vehicles = await _dataRetrievalService.RetriveVehicleDetailsAsync(_dataSetId, vehicleIds);
            Assert.AreEqual(vehicles.Length, 0);
        }

        [Test]
        public async Task RetriveDealerDetailsAsyc_ShouldReturnValidResults()
        {
            List<int> dealerIds = new List<int> {
                398860453,
                398860453,
                346042914
            };
            var dealers = await _dataRetrievalService.RetriveDealerDetailsAsyc(_dataSetId, dealerIds);
            Assert.Greater(dealers.Count, 0);
        }

    }
}
