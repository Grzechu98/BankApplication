using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BankApplication.Tests
{
    public class OperationControllerTest : IntegrationTest
    {
        [Fact]
        public async Task GetOperations_IntegrationTest()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Content = new StringContent("", Encoding.UTF8, "application/json"),
                RequestUri = new Uri("http://localhost:61004/api/Operations")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task GetOperation_IntegrationTest()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Content = new StringContent("", Encoding.UTF8, "application/json"),
                RequestUri = new Uri("http://localhost:61004/api/Operations/1")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostInternalTransfer_IntegrationTest()
        {
            var ToPost = new
            {
                Type = "Przelew Wewnetrzny",
                Title = "TEST",
                Status = "aaa",
                Value = 30,
                RecipientId = 1,
                SenderId = 2
            };

            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = JsonHelper.TransformToJson(ToPost),
                RequestUri = new Uri("http://localhost:61004/api/Operations/InternalTransfer")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
    }
}
