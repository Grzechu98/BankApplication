using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication.Services
{
    public class AccountNumberGenerator
    {
        public static string GetAccountNumber()
        {
            var CountryCode = "2521";
            var BankInternalNumber = "82442760";

            Random random = new Random();
            string characters = "0123456789";
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            result.ToString();
            var AccountNumberWithoutChecksum = BankInternalNumber + result + CountryCode;

            decimal number;

            number = decimal.Parse(AccountNumberWithoutChecksum);
            var Checksum = number % 97;
            var res = Checksum - 98;
            var CheckDigit = Decimal.ToInt32(Math.Abs(res));

            var AccountNumber = CheckDigit + BankInternalNumber + result;
            if (CheckDigit == 0)
            {
                AccountNumber = "00" + CheckDigit + BankInternalNumber + result + "ABC";
            }
            else if (CheckDigit <= 9)
            {
                AccountNumber = "0" + CheckDigit + BankInternalNumber + result;
            }
            else
            {
                AccountNumber = CheckDigit + BankInternalNumber + result;
            }

            return AccountNumber;
        }
    }
}
