using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankApplication.Data;
using BankApplication.Models;
using BankApplication.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BankApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MainContext _context;
        private readonly IEncrypter _encrypter;
        private readonly ITokenService _tokenService;

        public UsersController(MainContext context, IEncrypter encrypter, ITokenService tokenService)
        {
            _context = context;
            _encrypter = encrypter;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> Login([FromBody] JObject data)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Login == data["login"].ToString());
            var password = _encrypter.EncryptData(data["password"].ToString());
            if (user != null && user.Password == password)
            {
                var token = await _tokenService.GetToken(user.Id);

                return Ok(new
                {
                    user = user,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            user.Password = _encrypter.EncryptData(user.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserModel", new { id = user.Id }, user);
        }


        // GET: api/Users
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserModel>> GetUserModel()
        {
            var userModel = await _context.Users.Include(e => e.Address).FirstOrDefaultAsync(a => a.Id == Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));

            if (userModel == null)
            {
                return NotFound();
            }
            return Ok(userModel);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserModel(int id, UserModel userModel)
        {
            if (id != userModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var token = await _tokenService.GetToken(Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
            return Ok(userModel);
        }
        private bool UserModelExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
