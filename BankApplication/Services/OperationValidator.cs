using BankApplication.SharedLibrary.Models;
using BankApplication.SharedLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.Services
{
    public interface IOperationValidator
    {
        Task<bool> IsTransferAmountCorrect(OperationModel operation);
        Task<bool> HasUnusedLimit(OperationModel operation);
        Task<bool> HasDailyAmountUnusedLimit(OperationModel operation);
    }
    public class OperationValidator : IOperationValidator
    {
        private readonly MainContext _context;

        public OperationValidator(MainContext context)
        {
            _context = context;
        }

        public async Task<bool> HasDailyAmountUnusedLimit(OperationModel operation)
        {
            var operations = await _context.Operations.Where(e => e.SenderId == operation.SenderId && e.OperationDate >= DateTime.Now.AddDays(-1)).ToListAsync();
            var settings = await _context.AccountSettings.FirstOrDefaultAsync(e => e.BankAccountId == operation.SenderId);
            decimal total = 0;
            
            foreach (var item in operations)
            {
                total += item.Value;
            }

            var limit = settings.DailyOperationLimit - total;
            if (operation.Value <= limit)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

        }

        public async Task<bool> HasUnusedLimit(OperationModel operation)
        {
            var operations = await _context.Operations.Where(e => e.SenderId == operation.SenderId && e.OperationDate >= DateTime.Now.AddDays(-1)).ToListAsync();
            var settings = await _context.AccountSettings.FirstOrDefaultAsync(e => e.BankAccountId == operation.SenderId);
            
            if (operations.Count < settings.MaxDailyOperationsNumber)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> IsTransferAmountCorrect(OperationModel operation)
        {
            var settings = await _context.AccountSettings.FirstOrDefaultAsync(e => e.BankAccountId == operation.SenderId);

            if (operation.Value <= settings.SingleOperationLimit)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        private bool IsDateBetween(DateTime date)
        {
            bool a = date >= DateTime.Now.AddDays(-1) && date <= DateTime.Now;
            return a;
        }
    }
}
