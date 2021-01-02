using BankApplication.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BankApplication.Tests
{
    public class IntegrationTest
    {
        protected readonly HttpClient client;
        protected IntegrationTest()
        {
            var factory = new WebApplicationFactory<Startup>();
            client = factory.CreateClient();
        }
    }
}
