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

namespace BankApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly MainContext _context;
        private readonly IOperationValidator _validator;

        public OperationsController(MainContext context, IOperationValidator validator)
        {
            _context = context;
            _validator = validator;
        }
        // GET: api/Operations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationModel>> GetOperationModel(int id)
        {
            var operationModel = await _context.Operations.FindAsync(id);

            if (operationModel == null)
            {
                return NotFound();
            }

            return operationModel;
        }

        // POST: api/Operations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<OperationModel>> PostOperationModel(OperationModel operationModel)
        {
            _context.Operations.Add(operationModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOperationModel", new { id = operationModel.Id }, operationModel);
        }

        [HttpPost("InternalTransfer")]
        public async Task<ActionResult<OperationModel>> MakeInternalTransfer(OperationModel operationModel)
        {
            if (operationModel.RecipientId == null)
                return BadRequest();
            var recipient = await _context.BankAccounts.FirstOrDefaultAsync(e => e.Id == operationModel.RecipientId);
            var sender = await _context.BankAccounts.FirstOrDefaultAsync(e => e.Id == operationModel.SenderId);
            if (await _validator.HasUnusedLimit(operationModel) && await _validator.IsTransferAmountCorrect(operationModel) && await _validator.HasDailyAmountUnusedLimit(operationModel))
            {
                if (sender.Balance < operationModel.Value)
                {
                    return BadRequest("lack of account funds");
                }
                else
                {
                    recipient.Balance += operationModel.Value;
                    sender.Balance -= operationModel.Value;
                }
            }
            else
            {
                return BadRequest("limits exceeded");
            }
            _context.Operations.Add(operationModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOperationModel", new { id = operationModel.Id }, operationModel);
        }
  
    }
}
