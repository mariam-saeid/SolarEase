using AutoMapper;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolarEase.Models.DTO.AccountDto;
using System.Transactions;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAdminsController : ControllerBase
    {
        private readonly IAdminRepository adminRepository;
        private readonly UserManager<Account> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthAdminsController(IAdminRepository adminRepository, UserManager<Account> userManager, ITokenRepository tokenRepository)
        {
            this.adminRepository = adminRepository;
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginCompanyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(loginCompanyDto.Email);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginCompanyDto.Password); //return bool flag idicate if the password valid for the user
                if (checkPasswordResult)
                {
                    //get roles for this user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null && roles[0] == "Admin")
                    {
                        //create token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var adminModel = await adminRepository.GetByEmailAsync(loginCompanyDto.Email);
                        if (adminModel == null)
                        {
                            return NotFound("admin not found");
                        }

                        var adminResponseDto = adminModel.AdminResponseInfo();

                        adminResponseDto.JwtToken = jwtToken;

                        return Ok(adminResponseDto);

                    }
                }
            }
            return StatusCode(401, "Email or password incorrect"); //unauthorized
        }
    }
}
