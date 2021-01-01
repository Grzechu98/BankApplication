using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication.Services
{
    public interface IEncrypter
    {
        public string EncryptData(string data);
    }
    public class Encrypter : IEncrypter
    {
        private readonly string Key = "zAq!2WsXcVGy&*90p";

        public string EncryptData(string data)
        {
            if (string.IsNullOrEmpty(data)) return "";
            data += Key;
            var encryptedData = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(encryptedData);
        }
    }
}
