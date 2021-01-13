using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankApplication.SharedLibrary.Models;
using BankApplication.SharedLibrary.Data;
using Microsoft.AspNetCore.Authorization;

namespace BankApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountSettingsController : ControllerBase
    {
        private readonly MainContext _context;

        public AccountSettingsController(MainContext context)
        {
            _context = context;
        }
        // GET: api/AccountSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountSettings>> GetAccountSettings(int id)
        {
            var accountSettings = await _context.AccountSettings.FirstOrDefaultAsync(e => e.BankAccountId == id);

            if (accountSettings == null)
            {
                return NotFound();
            }

            return accountSettings;
        }

        // PUT: api/AccountSettings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountSettings(int id, AccountSettings accountSettings)
        {
            if (id != accountSettings.Id)
            {
                return BadRequest();
            }

            _context.Entry(accountSettings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountSettingsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool AccountSettingsExists(int id)
        {
            return _context.AccountSettings.Any(e => e.Id == id);
        }
    }
}
