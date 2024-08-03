using AutoMapper;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolarEase.Models.DTO.ProfileDto;
using SolarEase.Models.DTO.AccountDto;
using SolarEase.Services;
using SolarEase.Models.DTO.EmailDto;
using System.Transactions;
using SolarEase.Models.DTO.PersonDto;
using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Crypto.Macs;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthPersonsController : ControllerBase
    {
        private readonly IPersonRepository personRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IMapper mapper;
        private readonly UserManager<Account> userManager;
        private readonly IAccountRepository accountRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly FileService fileService;
        private readonly EmailService emailService;

        public AuthPersonsController(IPersonRepository personRepository, IProfileRepository profileRepository, IMapper mapper,
            UserManager<Account> userManager, IAccountRepository accountRepository,
            ITokenRepository tokenRepository, FileService fileService, EmailService emailService)
        {
            this.personRepository = personRepository;
            this.profileRepository = profileRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.accountRepository = accountRepository;
            this.tokenRepository = tokenRepository;
            this.fileService = fileService;
            this.emailService = emailService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterPersonDto registerPersonDto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        transactionScope.Dispose();
                        return BadRequest(ModelState);
                    }
                    var personModel = registerPersonDto.PersonRegister();

                    var account = new Account
                    {
                        UserName = registerPersonDto.Email,
                        Email = registerPersonDto.Email,
                        Type = "Person"
                    };

                    var createProfileDto = new CreateProfileDto
                    {
                        Type = "Person"
                    };

                    var registerResult = await userManager.CreateAsync(account, registerPersonDto.Password);
                    if (registerResult.Succeeded)
                    {
                        registerResult = await userManager.AddToRoleAsync(account, "Person");
                        if (registerResult.Succeeded)
                        {
                            var profileModel = createProfileDto.ProfileCreate();
                            profileModel = await profileRepository.CreateAsync(profileModel);
                            personModel.AccountId = account.Id;
                            personModel.ProfileId = profileModel.Id;
                            personModel = await personRepository.CreateAsync(personModel);

                            // send email
                            EmailDto emailDto = new EmailDto
                            {
                                To = registerPersonDto.Email,
                                Subject = "SolarEase Account",
                                Body = "SolarEase account created successfully"
                            };
                            emailService.SendEmail(emailDto);

                            transactionScope.Complete(); // Commit transaction
                            return Ok("Person was registered! please login.");
                        }
                        else
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return StatusCode(500, registerResult.Errors);
                        }
                    }
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, registerResult.Errors);
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
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
                    if (roles != null && roles[0] == "Person")
                    {
                        //create token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var personModel = await personRepository.GetByEmailAsync(loginCompanyDto.Email);
                        if (personModel == null)
                        {
                            return NotFound("person not found");
                        }

                        var personResponseDto = personModel.PersonResponseInfo();

                        personResponseDto.JwtToken = jwtToken;

                        return Ok(personResponseDto);
                    }
                }
            }
            return StatusCode(401, "Email or password incorrect"); //unauthorized
        }

        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(forgetPasswordDto.Email);
            if (user != null)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                user.ValidationCode = new string(Enumerable.Repeat(chars, 6)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                user.ValidationCodeExpiration = DateTime.UtcNow.AddMinutes(15); // Code expires in 15 minutes


                var updateResult = await accountRepository.UpdateValidationAsync(user.Id, user);

                if (!updateResult.Succeeded)
                {
                    return StatusCode(500, updateResult.Errors); // Handle account update failure
                }

                
                // send email
                EmailDto emailDto = new EmailDto
                {
                    To = forgetPasswordDto.Email,
                    Subject = "Forget Passward",
                    Body = $"Your Validation Code code is {user.ValidationCode} and will expire at 15 minutes",

                };
                emailService.SendEmail(emailDto);


                return Ok();
            }
            return NotFound("Email Not Found"); //unauthorized
        }

        [HttpPost]
        [Route("ValidateCode")]
        public async Task<IActionResult> ValidateCode([FromBody] ValidateCodeDto validateCodeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(validateCodeDto.Email);
            if (user != null)
            {
                if (user.ValidationCode == validateCodeDto.ValidationCode)
                {
                    if (DateTime.UtcNow <= user.ValidationCodeExpiration)
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(401, "Validation code expaired"); 
                    }
                }
                else
                {
                    return StatusCode(401, "Validation code incorrect"); 

                }
            }
            return NotFound("Email Not Found"); //unauthorized
        }

        [HttpPut]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
                if (user == null)
                {
                    return NotFound("Email not Found");
                }

                user.ValidationCode = null;
                user.ValidationCodeExpiration = null;

                var (updateResult, resetToken) = await accountRepository.UpdateAsync(user.Id, user, resetPasswordDto.Password);

                if (!updateResult.Succeeded)
                {
                    return StatusCode(500, updateResult.Errors); // Handle account update failure
                }

                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
