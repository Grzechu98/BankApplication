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
        public async Task<ActionResult<ICollection<IOperation>>> GetOperations()
        {
            var sid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var operationModel = await _context.Operations.Where(s => s.SenderId == sid || s.RecipientId == sid).ToListAsync();
            var eoperationModel = await _context.ExternalOperations.Where(e => e.TargetInternalAccountId == sid).ToListAsync();
           
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
            var list = new List<IOperation>();
            list.AddRange(operationModel); list.AddRange(eoperationModel);
            list.OrderByDescending(e => e.Id);
            return list;
        }


        // GET: api/Operations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationModel>> GetOperationModel(int id)
        {
            var operationModel = await _context.Operations.Include(o => o.Recipient).ThenInclude(r => r.User).Include(o => o.Sender).ThenInclude(r => r.User).FirstOrDefaultAsync(o => o.Id == id);

            if (operationModel == null)
            {
                return NotFound();
            }

            return operationModel;
        }
        // GET: api/Operations/5
        [HttpGet("External/{id}")]
        public async Task<ActionResult<ExternalOperationModel>> GetExternalOperationModel(int id)
        {
            var operationModel = await _context.ExternalOperations.Include(o=>o.TargetInternalAccount).ThenInclude(r => r.User).FirstOrDefaultAsync(o => o.Id == id);

            if (operationModel == null)
            {
                return NotFound();
            }

            return operationModel;
        }

        [HttpPost("InternalTransfer")]
        public async Task<ActionResult<OperationModel>> MakeInternalTransfer(OperationModel operationModel)
        {
            operationModel.OperationDate = DateTime.Now;
            var recipient = await _context.BankAccounts.FirstOrDefaultAsync(e => e.AccountNumber == operationModel.RecipientAccountNumber);
            operationModel.RecipientId = recipient.Id;
            var sender = await _context.BankAccounts.FirstOrDefaultAsync(e => e.Id == operationModel.SenderId);
            if (await _validator.HasUnusedLimit(operationModel.SenderId) && await _validator.IsTransferAmountCorrect(operationModel.SenderId, operationModel.Value) && await _validator.HasDailyAmountUnusedLimit(operationModel.SenderId, operationModel.Value))
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
        public async Task<ActionResult<ExternalOperationModel>> MakeExternalTransfer(ExternalOperationModel operationModel)
        {
            var sender = await _context.BankAccounts.FirstOrDefaultAsync(e => e.Id == operationModel.TargetInternalAccountId);
            if (await _validator.HasUnusedLimit(operationModel.TargetInternalAccountId) && await _validator.IsTransferAmountCorrect(operationModel.TargetInternalAccountId,operationModel.Value) && await _validator.HasDailyAmountUnusedLimit(operationModel.TargetInternalAccountId, operationModel.Value))
            {
                if (sender.Balance < operationModel.Value)
                {
                    return BadRequest("lack of account funds");
                }
                else
                {
                    //send request to settlement unit
                    sender.Balance -= operationModel.Value;
                }
            }
            else
            {
                return BadRequest("limits exceeded");
            }
            _context.ExternalOperations.Add(operationModel); 
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExternalOperationModel", new { id = operationModel.Id }, operationModel);
        }

    }
}
