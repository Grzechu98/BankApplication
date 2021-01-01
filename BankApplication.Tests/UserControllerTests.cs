using BankApplication.Models;
using BankApplication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BankApplication.Tests
{
    public class UserControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Register_IntegrationTest()
        {
            var request = new
            {
                Url = "api/Users/register",
                Body = new
                {
                    Name = "Test",
                    Secondname = "test",
                    Nationality = "test",
                    PlaceOfBirth = "33",
                    DateOfBirth = DateTime.Today,
                    PIN = "3333333333323333",
                    PhoneNumber = "12333333",
                    Email = "aaa@aaa.aaa",
                    IdentityDocumentNumber = "3211",
                    IdentityDocumentExpirationDate = DateTime.Now.AddDays(14),
                    AddressId = 1,
                    Login = "test1123",
                    Password = "123"
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
                    login = "test123",
                    password = "123"
                }
            };

            var response = await client.PostAsync(request.Url, JsonHelper.TransformToJson(request.Body));

            response.EnsureSuccessStatusCode();
        }
    }   
}
