using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankApplication.Data;
using BankApplication.Models;
using System.Security.Claims;
using BankApplication.Services;
using Microsoft.AspNetCore.Authorization;

namespace BankApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly MainContext _context;

        public BankAccountsController(MainContext context)
        {
            _context = context;
        }

        // GET: api/BankAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccountModel>>> GetBankAccounts()
        {
            var id = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return await _context.BankAccounts.Where(e => e.UserId == id).ToListAsync();
        }

        // GET: api/BankAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccountModel>> GetBankAccountModel(int id)
        {
            var uid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var bankAccountModel = await _context.BankAccounts.FirstOrDefaultAsync(e => e.UserId == uid && e.Id == id);

            if (bankAccountModel == null)
            {
                return NotFound();
            }

            return bankAccountModel;
        }

        // PUT: api/BankAccounts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("CloseAccount/{id}")]
        public async Task<IActionResult> PutBankAccountModel(int id)
        {
            var bankAccountModel = await _context.BankAccounts.FirstOrDefaultAsync(a => a.Id == id);
            if (!BankAccountModelExists(id))
            {
                return NotFound();
            }

            bankAccountModel.IsActive = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/BankAccounts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BankAccountModel>> PostBankAccountModel()
        {
            var BankAccount = new BankAccountModel
            {
                AccountNumber = AccountNumberGenerator.GetAccountNumber(),
                Balance = 0,
                IsActive = true,
                UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value),
                Settings = new AccountSettings { DailyOperationLimit = 10000, MaxDailyOperationsNumber = 15, SingleOperationLimit = 5000 },

            };
            _context.BankAccounts.Add(BankAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBankAccountModel", new { id = BankAccount.Id }, BankAccount);
        }
        private bool BankAccountModelExists(int id)
        {
            return _context.BankAccounts.Any(e => e.Id == id);
        }
    }
}
