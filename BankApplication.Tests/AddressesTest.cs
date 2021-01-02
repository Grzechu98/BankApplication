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
    public class AddressesTest : IntegrationTest
    {
        [Fact]
        public async Task PutAddress_IntegrationTest()
        {
            var ToPost = new
            {
                id = 2,
                Country = "Polska",
                City = "Rzeszów",
                Street = "Grabskiego",
                UnitNumber = "323",
                PostCode = "35-001"
            };

            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Content = JsonHelper.TransformToJson(ToPost),
                RequestUri = new Uri("http://localhost:61004/api/Addresses/2")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
    }
}
