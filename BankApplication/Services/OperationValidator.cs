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
        Task<bool> IsTransferAmountCorrect(int Id, decimal value);
        Task<bool> HasUnusedLimit(int Id);
        Task<bool> HasDailyAmountUnusedLimit(int Id, decimal value);
    }
    public class OperationValidator : IOperationValidator
    {
        private readonly MainContext _context;

        public OperationValidator(MainContext context)
        {
            _context = context;
        }

        public async Task<bool> HasDailyAmountUnusedLimit(int Id,decimal value)
        {            
            var operations = await _context.Operations.Where(e => e.SenderId == Id && e.OperationDate >= DateTime.Now.AddDays(-1)).ToListAsync();
            var eoperations = await _context.ExternalOperations.Where(e => e.TargetInternalAccountId == Id && e.OperationDate >= DateTime.Now.AddDays(-1)).ToListAsync();
            var list = new List<IOperation>();
            list.AddRange(operations); list.AddRange(eoperations);
            var settings = await _context.AccountSettings.FirstOrDefaultAsync(e => e.BankAccountId == Id);
            decimal total = 0;
            
            foreach (var item in list)
            {
                total += item.Value;
            }

            var limit = settings.DailyOperationLimit - total;
            if (value <= limit)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

        }

        public async Task<bool> HasUnusedLimit(int Id)
        {
            var operations = await _context.Operations.Where(e => e.SenderId == Id && e.OperationDate >= DateTime.Now.AddDays(-1)).ToListAsync();
            var eoperations = await _context.ExternalOperations.Where(e => e.TargetInternalAccountId == Id && e.OperationDate >= DateTime.Now.AddDays(-1)).ToListAsync();
            var settings = await _context.AccountSettings.FirstOrDefaultAsync(e => e.BankAccountId == Id);
            
            if ((operations.Count + eoperations.Count) < settings.MaxDailyOperationsNumber)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> IsTransferAmountCorrect(int Id, decimal value)
        {
            var settings = await _context.AccountSettings.FirstOrDefaultAsync(e => e.BankAccountId == Id);

            if (value <= settings.SingleOperationLimit)
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
