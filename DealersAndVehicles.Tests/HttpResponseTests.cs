using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DealersAndVehicles.Tests
{
    class HttpResponseTests
    {
        private HttpClient client;
        private HttpResponseMessage response;

        [SetUp]
        public void Setup()
        {
            client = new HttpClient();
            string baseUrl = ConfigurationManager.AppSetting["baseUrl"];
            response = client.GetAsync($"{baseUrl}/datasetId").Result;
        }

        [Test]
        public void GetResponseIsSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void GetResponseIsJson()
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Test]
        public void GetAuthenticationStatus()
        {
            Assert.AreNotEqual(HttpStatusCode.Unauthorized, response.StatusCode);

        }
    }
}
