using DealersAndVehicles.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DealersAndVehicles.Models;
using System.Net.Http;

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
        public async Task RetrieveVehicleIdsAsync_ShouldThrowExceptionForIncorrectDatasetID()
        {
            Assert.ThrowsAsync<UnsupportedMediaTypeException>(async () => await _dataRetrievalService.RetrieveVehicleIdsAsync("abcdefghijk"));
        }


        [Test]
        public async Task RetrieveVehicleIdsAsync_ShouldReturnVehicleListForValidDataSetId()
        {
            var vehicles = await _dataRetrievalService.RetrieveVehicleIdsAsync(_dataSetId);
            CollectionAssert.IsNotEmpty(vehicles);
        }

        [Test]
        public async Task RetrieveVehicleIdsAsync_ShouldReturnExceptionForNullDatasetId()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _dataRetrievalService.RetrieveVehicleIdsAsync(null));
        }

        [Test]
        public async Task RetriveVehicleDetailsAsync_ShouldReturnCorrectListForVehicles()
        {
            List<int> vehicleIds = new List<int>
            {
                384297551,
                2068036871,
                1781801989
            };

            var vehicles = await _dataRetrievalService.RetriveVehicleDetailsAsync(_dataSetId, vehicleIds);
            CollectionAssert.AreEquivalent(vehicleIds, vehicles.Select(i => i.vehicleId).ToList());
        }

        [Test]
        public async Task RetriveVehicleDetailsAsync_ShouldReturnEmptyForEmptyVehicleIds()
        {
            List<int> vehicleIds = new List<int>();
            VehicleResponse[] vehicles = await _dataRetrievalService.RetriveVehicleDetailsAsync(_dataSetId, vehicleIds);
            CollectionAssert.IsEmpty(vehicles);
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
            CollectionAssert.IsNotEmpty(dealers);
        }
    }
}
