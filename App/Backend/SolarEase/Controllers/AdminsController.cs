using AutoMapper;
using SolarEase.Services;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Models.DTO.AccountDto;
using SolarEase.Models.DTO.ProfileDto;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using SolarEase.Models.DTO.AdminDto;
using Newtonsoft.Json.Linq;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminRepository adminRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;
        private readonly IProfileRepository profileRepository;
        private readonly FileService fileService;
        private readonly UserManager<Account> userManager;

        public AdminsController(IAdminRepository adminRepository, IAccountRepository accountRepository, IMapper mapper, IProfileRepository profileRepository, FileService fileService, UserManager<Account> userManager)
        {
            this.adminRepository = adminRepository;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.profileRepository = profileRepository;
            this.fileService = fileService;
            this.userManager = userManager;
        }

        // admins info
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var adminsModel = await adminRepository.GetAllAsync();
            var adminsDto = adminsModel.AdminsInfo();   //dist-src
            return Ok(adminsDto);
        }

        //***************************************************************************************
        // admin info 
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetByIdInfo([FromRoute] int id)
        {
            var adminModel = await adminRepository.GetByIdAsync(id);
            if (adminModel == null)
            {
                return NotFound();
            }
            var adminDto = adminModel.AdminInfo();
            return Ok(adminDto);
        }

        //***************************************************************************************
        // create admin
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] CreateAdminDto createAdminDto)
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

                    var adminModel = createAdminDto.AdminCreate();

                    var account = new Account
                    {
                        UserName = createAdminDto.Email,
                        Email = createAdminDto.Email,
                        Type = "Admin"
                    };

                    var createProfileDto = new CreateProfileDto
                    {
                        Type = "Admin"
                    };

                    var registerResult = await userManager.CreateAsync(account, createAdminDto.Password);
                    if (registerResult.Succeeded)
                    {
                        registerResult = await userManager.AddToRoleAsync(account, "Admin");
                        if (registerResult.Succeeded)
                        {
                            var profileModel = createProfileDto.ProfileCreate();
                            profileModel = await profileRepository.CreateAsync(profileModel);
                            adminModel.AccountId = account.Id;
                            adminModel.ProfileId = profileModel.Id;
                            adminModel = await adminRepository.CreateAsync(adminModel);

                            transactionScope.Complete(); // Commit transaction
                            return Ok("Admin Created Successfully");
                        }
                        else
                        {
                            transactionScope.Dispose();
                            return StatusCode(501, registerResult.Errors);
                        }
                    }
                    transactionScope.Dispose();
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


        //***************************************************************************************
        // update admin info - image - account
        [HttpPut]
        [Route("UpdateInfo/{id:int}")]
        public async Task<IActionResult> UpdateInfo([FromRoute] int id, [FromForm] UpdateAdminInfoDto updateAdminInfo2Dto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Map/convert DTO to domain model, Update all parameters
                        var adminModel = updateAdminInfo2Dto.AdminUpdateInfo();

                        //Update CVurl parameters
                        var old = await adminRepository.GetByIdAsync(id);

                        // Update database
                        var updatedAdmin = await adminRepository.UpdateAsync(id, adminModel);
                        if (updatedAdmin == null)
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return NotFound("admin not found");
                        }

                        // Update profile information
                        var updateProfileDto = new CreateProfileDto
                        {
                            Image = updateAdminInfo2Dto.Image,
                            Type = "Admin"
                        };
                        var profileModel = updateProfileDto.ProfileCreate();
                        profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateProfileDto.Image, old.Profile.ImageUrl);
                        profileModel = await profileRepository.UpdateAsync(updatedAdmin.ProfileId, profileModel);

                        // Update account information
                        var accountDomainModel = new Account
                        {
                            UserName = updateAdminInfo2Dto.Email,
                            Email = updateAdminInfo2Dto.Email
                        };
                        var updatedAccount = await accountRepository.GetByIdAsync(updatedAdmin.AccountId);
                        if (updatedAccount == null)
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return NotFound("Account not Found");
                        }

                        var (updateResult, resetToken) = await accountRepository.UpdateAsync(updatedAdmin.AccountId, accountDomainModel, updateAdminInfo2Dto.Password);

                        if (!updateResult.Succeeded)
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return BadRequest(updateResult.Errors); // Handle account update failure
                        }


                        var adminResponseDto = adminModel.AdminInfo();
                        transactionScope.Complete(); // Commit transaction
                        return Ok(adminResponseDto);
                    }
                    else
                    {
                        transactionScope.Dispose();
                        return BadRequest(ModelState);
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }

        //***************************************************************************************
        // update admin image
        [HttpPut]
        [Route("UpdateAdminProfile/{id:int}")]
        public async Task<IActionResult> UpdateAdminProfile([FromRoute] int id, [FromForm] UpdateProfileDto updateProfileDto)
        {
            if (ModelState.IsValid)
            {
                var old = await adminRepository.GetByIdAsync(id);
                if (old == null)
                {
                    return NotFound();
                }

                var profileModel = new Models.Domain.Profile(); //update type
                profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateProfileDto.Image, old.Profile.ImageUrl); //update image,solved
                profileModel = await profileRepository.UpdateAsync(old.ProfileId, profileModel);

                return Ok("Admin Image Updated Successfully");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //***************************************************************************************
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var adminModel = await adminRepository.DeleteAsync(id);

                    if (adminModel == null) { return NotFound(); }

                    var profile = await profileRepository.DeleteAsync(adminModel.ProfileId);
                    fileService.DeleteFile(profile.ImageUrl);

                    var result = await accountRepository.DeleteAsync(adminModel.AccountId);

                    if (!result)
                    {
                        transactionScope.Dispose(); // Rollback transaction
                        return BadRequest(ModelState);
                    }

                    transactionScope.Complete(); // Commit transaction
                    return Ok("Admin Deleted Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }
    }
}
