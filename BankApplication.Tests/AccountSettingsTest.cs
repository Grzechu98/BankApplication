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
    public class AccountSettingsTest : IntegrationTest
    {
        [Fact]
        public async Task GetBankAccount_IntegrationTest()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Content = new StringContent("", Encoding.UTF8, "application/json"),
                RequestUri = new Uri("http://localhost:61004/api/AccountSettings/2")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task PutUser_IntegrationTest()
        {
            var ToPut = new
            {
                id = 2,
                MaxDailyOperationsNumber = 8,
                SingleOperationLimit = 5000,
                DailyOperationLimit = 10000,
                BankAccountId = 2
            };


            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Content = JsonHelper.TransformToJson(ToPut),
                RequestUri = new Uri("http://localhost:61004/api/AccountSettings/2")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
    }
}
