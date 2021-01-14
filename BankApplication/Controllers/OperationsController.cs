using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankApplication.SharedLibrary.Models;
using BankApplication.SharedLibrary.Data;
using System.Security.Claims;
using BankApplication.Services;
using Microsoft.AspNetCore.Authorization;

namespace BankApplication.Controllers
{
    [Authorize]
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

        [HttpGet]
        public async Task<ActionResult<ICollection<OperationModel>>> GetOperations(int id)
        {
            var sid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var operationModel = await _context.Operations.Where(s => s.SenderId == sid || s.RecipientId == sid).ToListAsync();
            foreach (var item in operationModel)
            {
                if (item.SenderId == sid)
                {
                    item.Incoming = false;
                }
                else
                {
                    item.Incoming = true;
                }
            }

            return operationModel;
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

        [HttpPost("InternalTransfer")]
        public async Task<ActionResult<OperationModel>> MakeInternalTransfer(OperationModel operationModel)
        {
            if (operationModel.RecipientId == null)
                return BadRequest();
            operationModel.OperationDate = DateTime.Now;
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
            try
            {
                _context.Operations.Add(operationModel);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return BadRequest("error");
            }
            

            return CreatedAtAction("GetOperationModel", new { id = operationModel.Id }, operationModel);
        }
        [HttpPost("ExternalTransfer")]
        public async Task<ActionResult<OperationModel>> MakeExternalTransfer(OperationModel operationModel)
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
                    //TODO
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
