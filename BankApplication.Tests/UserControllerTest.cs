using BankApplication.Controllers;
using BankApplication.Models;
using BankApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BankApplication.Tests
{
    public class UserControllerTest : IntegrationTest
    {

        [Fact]
        public async Task Register_IntegrationTest()
        {
            var request = new
            {
                Url = "api/Users/register",
                Body = new
                {
                    Name = "Testa",
                    Secondname = "testa",
                    Nationality = "test",
                    PlaceOfBirth = "323",
                    DateOfBirth = DateTime.Today,
                    PIN = "3333333333323333",
                    PhoneNumber = "12333333",
                    Email = "aaa@aaa.aaaa",
                    IdentityDocumentNumber = "1112",
                    IdentityDocumentExpirationDate = DateTime.Now.AddDays(14),
                    Address =new {
                        Country = "abc",
                        City= "cdf",
                        Street = "aaa",
                        UnitNumber = "bbb",
                        PostCode = "32-222"
                    },
                    Login = "test12123",
                    Password = "1233"
                }
            };

            var response = await client.PostAsync(request.Url, JsonHelper.TransformToJson(request.Body));

            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Login_IntegrationTest()
        {
            var request = new
            {
                Url = "api/Users/login",
                Body = new
                {
                    login = "test1231",
                    password = "123"
                }
            };

            var response = await client.PostAsync(request.Url, JsonHelper.TransformToJson(request.Body));

            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task GetUser_IntegrationTest()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Content = new StringContent("",Encoding.UTF8, "application/json"),
                RequestUri = new Uri("http://localhost:61004/api/Users")
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
               Name = "Testa",
               Secondname = "testa",
               Nationality = "test",
               PlaceOfBirth = "323",
               DateOfBirth = DateTime.Today,
               PIN = "3333333333323333",
               PhoneNumber = "12333333",
               Email = "aaa@aaa.12aaaa",
               IdentityDocumentNumber = "1112",
               IdentityDocumentExpirationDate = DateTime.Now.AddDays(14),
               Login = "test12123"
           };
          

            var config = new ConfigurationBuilder()
            .AddJsonFile("jwtconfigtests.json").Build();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Content = JsonHelper.TransformToJson(ToPut),
                RequestUri = new Uri("http://localhost:61004/api/Users")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["JWTToken"]);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
    }   
}
