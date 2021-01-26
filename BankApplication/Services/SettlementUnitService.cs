using BankApplication.SharedLibrary.Data;
using BankApplication.SharedLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Newtonsoft.Json;

namespace BankApplication.Services
{
    public interface ISettlementUnitService
    {
        public Task PrepareTransfer(ExternalOperationModel operationModel);
        public Task MakeTransfer(ExternalTransferHelper transfer);
        public Task SettlementExternalTransfers();
    }
    public class SettlementUnitService : ISettlementUnitService
    {
        private readonly IConfiguration _configuration;
        private readonly MainContext _context;

        public SettlementUnitService(IConfiguration configuration, MainContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task SettlementExternalTransfers()
        {
            var transfers = await GetExternalTransfers();
            if (transfers.Any())
            {
                foreach (var item in transfers)
                {
                    if (!item.Title.Equals("Zwrot - przelew odrzucony"))
                    {
                        ExternalOperationModel externalOperation = new ExternalOperationModel();
                        externalOperation.Type = "Przelew Zewnętrzny";
                        externalOperation.Title = item.Title;
                        externalOperation.OperationDate = item.Date;
                        externalOperation.Incoming = true;
                        externalOperation.RecipientAccountNumber = item.SenderAccountNumber;
                        externalOperation.FullName = item.SenderName;
                        externalOperation.Value = item.Value;
                        externalOperation.TargetInternalAccount = _context.BankAccounts.Where(e => e.AccountNumber == item.RecipientAccountNumber).FirstOrDefault();
                        externalOperation.TargetInternalAccountId = externalOperation.TargetInternalAccount.Id;

                        externalOperation.TargetInternalAccount.Balance += item.Value;
                        await _context.ExternalOperations.AddAsync(externalOperation);
                    }
                    else
                    {
                        ExternalOperationModel externalOperation = new ExternalOperationModel();
                        externalOperation.Type = "Przelew Zewnętrzny";
                        externalOperation.Title = item.Title;
                        externalOperation.OperationDate = item.Date;
                        externalOperation.Incoming = true;
                        externalOperation.RecipientAccountNumber = item.RecipientAccountNumber;
                        externalOperation.FullName = item.RecipientName;
                        externalOperation.Value = item.Value;
                        externalOperation.TargetInternalAccount = _context.BankAccounts.Where(e => e.AccountNumber == item.SenderAccountNumber).FirstOrDefault();
                        externalOperation.TargetInternalAccountId = externalOperation.TargetInternalAccount.Id;

                        externalOperation.TargetInternalAccount.Balance += item.Value;
                        await _context.ExternalOperations.AddAsync(externalOperation);
                    }
                }
                await _context.SaveChangesAsync();

            }
        }

        public async Task MakeTransfer(ExternalTransferHelper transfer)
        { 
            HttpClient client = new HttpClient();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(transfer), Encoding.UTF8, "application/json"),
                RequestUri = new Uri(_configuration["SettlementUnitAddress"])
            };
            await client.SendAsync(requestMessage);
        }

        public async Task PrepareTransfer(ExternalOperationModel operationModel)
        {
            var senderhelper = await _context.BankAccounts.Include(e => e.User).Where(e => e.Id == operationModel.TargetInternalAccountId).FirstOrDefaultAsync();
            ExternalTransferHelper transferHelper = new ExternalTransferHelper()
            {
                Title = operationModel.Title,
                Date = operationModel.OperationDate,
                Value = operationModel.Value,
                SenderName = senderhelper.User.Name + " " + senderhelper.User.Secondname,
                RecipientName = operationModel.FullName,
                RecipientAccountNumber = operationModel.RecipientAccountNumber,
                SenderAccountNumber = senderhelper.AccountNumber
            };
            await MakeTransfer(transferHelper);
        }

        private async Task<IEnumerable<ExternalTransferHelper>> GetExternalTransfers()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_configuration["SettlementUnitAddress"] + _configuration["BankCode"]);

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            return await response.Content.ReadFromJsonAsync<IEnumerable<ExternalTransferHelper>>(options); 
        }

    }
}
