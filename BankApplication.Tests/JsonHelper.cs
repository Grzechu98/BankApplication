using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
namespace BankApplication.Tests
{
    public static class JsonHelper
    {
        public static StringContent TransformToJson(object obj) => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");

    }
}
