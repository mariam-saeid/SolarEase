using AutoMapper;
using SolarEase.Services;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Models.DTO.AccountDto;
using SolarEase.Models.DTO.ProfileDto;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using SolarEase.Models.DTO.PersonDto;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonRepository personRepository;
        private readonly IPostRepository postRepository;
        private readonly ICalculatorRepository calculatorRepository;
        private readonly ISolarProductRepository solarProductRepository;
        private readonly IFavoriteProductRepository favoriteProductRepository;
        private readonly IFavoritePostRepository favoritePostRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;
        private readonly IProfileRepository profileRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IChatbotMessageRepository chatbotMessageRepository;
        private readonly FileService fileService;

        public PersonsController(IPersonRepository personRepository,IPostRepository postRepository,
            ICalculatorRepository calculatorRepository, ISolarProductRepository solarProductRepository, 
            IFavoriteProductRepository favoriteProductRepository, IFavoritePostRepository favoritePostRepository,
            IAccountRepository accountRepository, IMapper mapper, IProfileRepository profileRepository,
            IMessageRepository messageRepository,IChatbotMessageRepository chatbotMessageRepository,FileService fileService)
        {
            this.personRepository = personRepository;
            this.postRepository = postRepository;
            this.calculatorRepository = calculatorRepository;
            this.solarProductRepository = solarProductRepository;
            this.favoriteProductRepository = favoriteProductRepository;
            this.favoritePostRepository = favoritePostRepository;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.profileRepository = profileRepository;
            this.messageRepository = messageRepository;
            this.chatbotMessageRepository = chatbotMessageRepository;
            this.fileService = fileService;
        }

        // persons info
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var personsModel = await personRepository.GetAllAsync();
            var personsDto = personsModel.PersonsInfo();
            return Ok(personsDto);
        }

        //***************************************************************************************

        // person info
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var personModel = await personRepository.GetByIdAsync(id);
            if (personModel == null)
            {
                return NotFound();
            }
            var personDto = personModel.PersonInfo();
            return Ok(personDto);
        }

        //***************************************************************************************

        // update person info - image - account
        [HttpPut]
        [Authorize]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdatePersonInfoDto updatePersonInfoDto)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Map/convert DTO to domain model, Update all parameters
                        var personModel = updatePersonInfoDto.PersonUpdateInfo();

                        //Update CVurl parameters
                        var old = await personRepository.GetByIdAsync(id);

                        // Update database
                        var updatedPerson = await personRepository.UpdateAllAsync(id, personModel);
                        if (updatedPerson == null)
                        {
                            return NotFound("person not found");
                        }

                        if (updatePersonInfoDto.ImageUpdated)
                        {
                            // Update profile information
                            var updateProfileDto = new CreateProfileDto
                            {
                                Image = updatePersonInfoDto.Image,
                                Type = "Person"
                            };
                            var profileModel = updateProfileDto.ProfileCreate();
                            profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateProfileDto.Image, old.Profile.ImageUrl);
                            profileModel = await profileRepository.UpdateAsync(updatedPerson.ProfileId, profileModel);
                        }

                        // Update account information
                        var accountDomainModel = new Account
                        {
                            UserName = updatePersonInfoDto.Email,
                            Email = updatePersonInfoDto.Email
                        };
                        var updatedAccount = await accountRepository.GetByIdAsync(updatedPerson.AccountId);
                        if (updatedAccount == null)
                        {
                            return NotFound("Account not Found");
                        }

                        var (updateResult, resetToken) = await accountRepository.UpdateAsync(updatedPerson.AccountId, accountDomainModel, updatePersonInfoDto.Password);

                        if (!updateResult.Succeeded)
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return StatusCode(500, updateResult.Errors); // Handle account update failure
                        }

                        transactionScope.Complete(); // Commit transaction

                       var personDto = updatedPerson.PersonInfo();
                        return Ok(personDto);
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

        // update person image
        [HttpPut]
        [Route("UpdatePersonProfile/{id:int}")]
        public async Task<IActionResult> UpdatePersonProfile([FromRoute] int id, [FromForm] UpdateProfileDto updateProfileDto)
        {
            if (ModelState.IsValid)
            {
                //Update CVurl parameters
                var old = await personRepository.GetByIdAsync(id);
                if (old == null)
                {
                    return NotFound();
                }

                var profileModel = new Models.Domain.Profile(); //update type
                profileModel.ImageUrl = fileService.UpdateFile("ProfileImages", updateProfileDto.Image, old.Profile.ImageUrl); //update image,solved
                profileModel = await profileRepository.UpdateAsync(old.ProfileId, profileModel);

                return Ok("Person Image Updated Successfully");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //***************************************************************************************
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var person = await personRepository.GetByIdAsync(id);
                    if (person == null)
                    {
                        transactionScope.Dispose();
                        return NotFound("person not found");
                    }

                    //delete favoriteProducts
                    var favoriteProducts = await favoriteProductRepository.DeleteAllByPersonAsync(id);
                    
                    //delete favoritePosts
                    var favoritePosts = await favoritePostRepository.DeleteAllByPersonAsync(id);
                    // delete posts
                    var posts = await postRepository.GetAllPersonAsync(id);
                    foreach(var post in posts)
                    {
                        // delete favoritePosts for posts
                        var favorites = await favoritePostRepository.DeleteAllByPostAsync(post.Id);

                        var postModel = await postRepository.DeleteAsync(post.Id);

                        if (postModel == null)
                        {
                            transactionScope.Dispose();
                            return NotFound("post not found");
                        }
                        // delete solarProducts for posts
                        var solarProduct = await solarProductRepository.DeleteAsync(postModel.SolarProductId);
                        if (solarProduct == null)
                        {
                            transactionScope.Dispose();
                            return NotFound("product not found");
                        }
                        fileService.DeleteFile(solarProduct.ImageUrl);
                    }
                    //delete calculator
                    var calculators = await calculatorRepository.DeleteAllByPersonAsync(id);

                    //delete chatbotHistoryMessages
                    var chatbotMessages = await chatbotMessageRepository.DeleteAllByPersonAsync(id);

                    //delete messages
                    var messages = await messageRepository.DeleteAllByPersonAsync(id);

                    //delete person
                    var personModel = await personRepository.DeleteAsync(id);

                    if (personModel == null) {
                        transactionScope.Dispose(); 
                        return NotFound(); 
                    }

                    //delete profile
                    var profile = await profileRepository.DeleteAsync(personModel.ProfileId);
                    fileService.DeleteFile(profile.ImageUrl);

                    //delete account
                    var result = await accountRepository.DeleteAsync(personModel.AccountId);

                    if (!result)
                    {
                        transactionScope.Dispose(); // Rollback transaction
                        return BadRequest(ModelState);
                    }

                    transactionScope.Complete(); // Commit transaction
                    return Ok("Person Deleted Successfully");
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
