using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;
using System.Web.Http.Results;
using WeatherData.Controllers;
using WeatherData.Models;

namespace WeaterData.Test
{
    /// <summary>
    /// Tests for <see cref="WeatherController"/>
    /// </summary>
    [TestFixture]
    public class WeatherControllerTest
    {
        private WeatherController weatherController;
        private Mock<HttpClient> httpClient;
        private Mock<ILogger<WeatherController>> logger;

        [SetUp]
        public void Setup()
        {
            httpClient = new Mock<HttpClient>();
            logger = new Mock<ILogger<WeatherController>>();
            weatherController = new WeatherController(logger.Object, httpClient.Object);
        }

        /// <summary>
        /// Tests that a valid request can go thorugh to the end of the method, parsing mocked results etc.
        /// </summary>
        [TestMethod]
        public async void TestValidRequest()
        {
            //SETUP
            //TODO: Use Fixture to build parameters used in mocks.
            double latitude = 55.7029296;
            double longitude = 13.1929449;
            InformationType type = InformationType.Current;
            string apiKey = "apiKey";
            HttpResponseMessage locationResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            string locationJson = "[\r\n    {\r\n        \"name\": \"Lund\",\r\n        \"local_names\": {\r\n            \"da\": \"Lund\",\r\n            \"ko\": \"룬드\",\r\n            \"zh\": \"隆德\",\r\n            \"ru\": \"Лунд\",\r\n            \"sv\": \"Lund\",\r\n            \"ja\": \"ルンド\",\r\n            \"nn\": \"Lund\",\r\n            \"no\": \"Lund\"\r\n        },\r\n        \"lat\": 55.7029296,\r\n        \"lon\": 13.1929449,\r\n        \"country\": \"SE\"\r\n    }\r\n]";
            locationResponse.Content = new StringContent(locationJson, Encoding.UTF8, "application/json");

            HttpResponseMessage weatherResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            string weatherJson = "{\r\n    \"coord\": {\r\n        \"lon\": 13.1929,\r\n        \"lat\": 55.7029\r\n    },\r\n    \"weather\": [\r\n        {\r\n            \"id\": 803,\r\n            \"main\": \"Clouds\",\r\n            \"description\": \"broken clouds\",\r\n            \"icon\": \"04d\"\r\n        }\r\n    ],\r\n    \"base\": \"stations\",\r\n    \"main\": {\r\n        \"temp\": 294.16,\r\n        \"feels_like\": 293.99,\r\n        \"temp_min\": 292.86,\r\n        \"temp_max\": 294.76,\r\n        \"pressure\": 1016,\r\n        \"humidity\": 64,\r\n        \"sea_level\": 1016,\r\n        \"grnd_level\": 1010\r\n    },\r\n    \"visibility\": 10000,\r\n    \"wind\": {\r\n        \"speed\": 5.38,\r\n        \"deg\": 225,\r\n        \"gust\": 6.27\r\n    },\r\n    \"clouds\": {\r\n        \"all\": 82\r\n    },\r\n    \"dt\": 1691927145,\r\n    \"sys\": {\r\n        \"type\": 1,\r\n        \"id\": 1760,\r\n        \"country\": \"SE\",\r\n        \"sunrise\": 1691897678,\r\n        \"sunset\": 1691952594\r\n    },\r\n    \"timezone\": 7200,\r\n    \"id\": 2693678,\r\n    \"name\": \"Lund\",\r\n    \"cod\": 200\r\n}";
            weatherResponse.Content = new StringContent(weatherJson, Encoding.UTF8, "application/json");

            httpClient.Setup(
                m => m.GetAsync(
                    $"http://api.openweathermap.org/geo/1.0/reverse?lat={latitude}&lon={longitude}&limit=1&appid={apiKey}",
                    CancellationToken.None))
            .ReturnsAsync(locationResponse);

            httpClient.Setup(
                m => m.GetAsync(
                    $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}",
                    CancellationToken.None))
            .ReturnsAsync(weatherResponse);

            WeatherSummary expected = new WeatherSummary(
                new Location { City = "Lund", Country = "SE" },
                type = InformationType.Current,
                new WeatherInfoList() { List = new List<WeatherInfo>() { new WeatherInfo() {
                    Weather = new List<Weather>() { new Weather() { MainWeather = "Cloudy" } },
                    MainInfo = new MainInfo() { Temperature = 294.16, Pressure = 1016, Humidity = 64},
                    Wind = new Wind() { Speed = 5.38},
                    Date = null}}});

            //TEST
            var result = await weatherController.Get(latitude, longitude, type, CancellationToken.None);

            //ASSERT
            NUnit.Framework.Assert.Equals(result.Result, expected);
        }

        [TestMethod]
        public async Task TestUnAllowedLatitudeAsync()
        {
            //SETUP
            double latitude = 9001;
            double longitude = 13.1929449;
            InformationType type = InformationType.Current;

            //TEST 
            var result = await weatherController.Get(latitude, longitude, type, CancellationToken.None);

            //ASSERT
            NUnit.Framework.Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [TestMethod]
        public async Task TestOpenWeatherMapReturnsBadJson()
        {
            //SETUP
            //TODO: Use Fixture to build parameters used in mocks.
            double latitude = 55.7029296;
            double longitude = 13.1929449;
            InformationType type = InformationType.Current;
            string apiKey = "apiKey";
            HttpResponseMessage locationResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            string locationJson = "thisisnotjsonformat";
            locationResponse.Content = new StringContent(locationJson, Encoding.UTF8, "application/json");

            httpClient.Setup(
                m => m.GetAsync(
                    $"http://api.openweathermap.org/geo/1.0/reverse?lat={latitude}&lon={longitude}&limit=1&appid={apiKey}",
                    CancellationToken.None))
            .ReturnsAsync(locationResponse);

            var expected = (int)HttpStatusCode.Unauthorized;

            //TEST
            var result = await weatherController.Get(latitude, longitude, type, CancellationToken.None);

            //ASSERT
            NUnit.Framework.Assert.Equals(result.Result, expected);
        }

    }
}
