using AutoMapper;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Models.DTO.AccountDto;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;
        IMapper mapper;

        public AccountsController(IAccountRepository accountRepository, IMapper mapper)
        {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetALl()
        {
            var accounts = await accountRepository.GetAllAsync();
            return Ok(accounts);
        }

        //***************************************************************************************
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var account = await accountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        //***************************************************************************************
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string userId)
        {
            var result = await accountRepository.DeleteAsync(userId);
            if (result)
            {
                return Ok("User deleted successfully.");
            }
            else
            {
                return NotFound("User not found.");
            }
        }
    
    }
}
