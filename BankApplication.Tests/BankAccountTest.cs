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
    public class BankAccountTest : IntegrationTest
    {
        [Fact]
        public async Task Post_IntegrationTest()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent("", Encoding.UTF8, "application/json"),
                RequestUri = new Uri("http://localhost:61004/api/BankAccounts")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task CloseBankAccount_IntegrationTest()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Content = new StringContent("", Encoding.UTF8, "application/json"),
                RequestUri = new Uri("http://localhost:61004/api/BankAccounts/CloseAccount/3")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task GetAllUsersBankAccounts_IntegrationTest()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Content = new StringContent("", Encoding.UTF8, "application/json"),
                RequestUri = new Uri("http://localhost:61004/api/BankAccounts")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task GetBankAccount_IntegrationTest()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Content = new StringContent("", Encoding.UTF8, "application/json"),
                RequestUri = new Uri("http://localhost:61004/api/BankAccounts/2")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
    }
}