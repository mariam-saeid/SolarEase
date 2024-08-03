using AutoMapper;
using SolarEase.Services;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using SolarEase.Models.DTO.PostDto;
using Microsoft.Extensions.Hosting;
using SolarEase.Models.DTO.EmailDto;
using SolarEase.Models.DTO.PersonDto;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly FileService fileService;
        private readonly IPostRepository postRepository;
        private readonly IFavoritePostRepository favoritePostRepository;
        private readonly IPersonRepository personRepository;
        private readonly IMessageRepository messageRepository;
        private readonly ISolarProductRepository solarProductRepository;
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly EmailService emailService;

        public PostsController(IMapper mapper,FileService fileService,IPostRepository postRepository,
            IFavoritePostRepository favoritePostRepository,IPersonRepository personRepository, IMessageRepository messageRepository,
            ISolarProductRepository solarProductRepository, IProductCategoryRepository productCategoryRepository,
             EmailService emailService)
        {
            this.mapper = mapper;
            this.fileService = fileService;
            this.postRepository = postRepository;
            this.favoritePostRepository = favoritePostRepository;
            this.personRepository = personRepository;
            this.messageRepository = messageRepository;
            this.solarProductRepository = solarProductRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.emailService = emailService;
        }

        // posts info
        [HttpGet]
        public async Task<IActionResult> GetAll(string? sortBy, String? categoryQuery, String? cityQuery)
        {
            var postsModel = await postRepository.GetAllQueryAsync(sortBy, categoryQuery, cityQuery);
            var postsDto = postsModel.PostsInfo();
            return Ok(postsDto);
        }

        // active small posts info info
        [HttpGet]
        [Route("SmallPosts/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> GetAllActiveSmallPosts([FromRoute] int personId, string? sortBy, String? categoryQuery, String? cityQuery)
        {
            var existingPerson = await personRepository.GetByIdAsync(personId);
            if (existingPerson == null)
            {
                return NotFound("person not found");
            }
            var postsModel = await postRepository.GetAllActiveQueryAsync(personId,sortBy, categoryQuery, cityQuery);
            var personSmallPostsDto = postsModel.SmallPostDtoInfo();
            
            foreach(var personSmallPostDto in personSmallPostsDto)
            {
                var favoritePostModel = await favoritePostRepository.GetByIdAsync(personId, personSmallPostDto.Id);
                if (favoritePostModel == null)
                {
                    personSmallPostDto.IsFavorite = false;
                }
                else
                {
                    personSmallPostDto.IsFavorite = true;
                }
            }
            return Ok(personSmallPostsDto);
        }

        // posts inactive info
        [HttpGet]
        [Route("InActive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllInActive(string? sortBy, String? categoryQuery, String? cityQuery)
        {
            var postsModel = await postRepository.GetAllInActiveQueryAsync(sortBy, categoryQuery, cityQuery);
            var postsDto = postsModel.PostsInfo();
            return Ok(postsDto);
        }

        // person posts info
        [HttpGet]
        [Route("PersonInfo/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> GetAllPersonInfo([FromRoute] int personId, string? sortBy, String? categoryQuery, String? cityQuery)
        {
            var existingPerson = await personRepository.GetByIdAsync(personId);
            if (existingPerson == null)
            {
                return NotFound("person not found");
            }
            var postsModel = await postRepository.GetAllPersonAsync(personId, sortBy, categoryQuery, cityQuery);
            var postsDto = postsModel.PostsInfo();
            return Ok(postsDto);
        }

        // person posts active info
        [HttpGet]
        [Route("PersonActive/{personId:int}")]
        public async Task<IActionResult> GetAllPersonActive([FromRoute] int personId, string? sortBy, String? categoryQuery, String? cityQuery)
        {
            var existingPerson = await personRepository.GetByIdAsync(personId);
            if (existingPerson == null)
            {
                return NotFound("person not found");
            }
            var postsModel = await postRepository.GetAllPersonActiveQueryAsync(personId, sortBy, categoryQuery, cityQuery);
            var postsDto = postsModel.PostsInfo();
            return Ok(postsDto);
        }

        // person posts inactive info
        [HttpGet]
        [Route("PersonInActive/{personId:int}")]
        public async Task<IActionResult> GetAllPersonInActive([FromRoute] int personId, string? sortBy, String? categoryQuery, String? cityQuery)
        {
            var existingPerson = await personRepository.GetByIdAsync(personId);
            if (existingPerson == null)
            {
                return NotFound("person not found");
            }
            var postsModel = await postRepository.GetAllPersonInActiveQueryAsync(personId, sortBy, categoryQuery, cityQuery);
            var postsDto = postsModel.PostsInfo();
            return Ok(postsDto);
        }

        //***************************************************************************************
        // post info
        [HttpGet]
        [Authorize]
        [Route("Info/{id:int}")]
        public async Task<IActionResult> GetByIdInfo([FromRoute] int id)
        {
            var postModel = await postRepository.GetByIdInfoAsync(id);
            if (postModel == null)
            {
                return NotFound();
            }
            var postDto = postModel.PostInfo();
            return Ok(postDto);
        }

        // post active info
        [HttpGet]
        [Route("ActiveInfo/{id:int}")]
        public async Task<IActionResult> GetByIdActiveInfo([FromRoute] int id)
        {
            var postModel = await postRepository.GetByIdActiveInfoAsync(id);
            if (postModel == null)
            {
                return NotFound();
            }
            var postDto = postModel.PostInfo();
            return Ok(postDto);
        }

        // post active info
        [HttpGet]
        [Route("InActiveInfo/{id:int}")]
        public async Task<IActionResult> GetByIdInActiveInfo([FromRoute] int id)
        {
            var postModel = await postRepository.GetByIdInActiveInfoAsync(id);
            if (postModel == null)
            {
                return NotFound();
            }
            var postDto = postModel.PostInfo();
            return Ok(postDto);
        }

        //***************************************************************************************
        // create post
        [HttpPost]
        [Route("Create/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int personId, [FromForm] CreatePostDto createPostDto)
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
                    var existingPerson = await personRepository.GetByIdAsync(personId);
                    if (existingPerson == null)
                    {
                        transactionScope.Dispose();
                        return NotFound("person not found");
                    }

                    var postModel = createPostDto.CreatePost();
                    postModel.PersonId = existingPerson.Id;

                    // CategoryNameExist
                    var productCategoryModel = await productCategoryRepository.GetByNameAsync(createPostDto.CategoryName);
                    if (productCategoryModel == null)
                    {
                        transactionScope.Dispose();
                        return NotFound("category not found");
                    }
                    var solarProduct = new SolarProduct
                    {
                        Price = createPostDto.Price,
                        Brand = createPostDto.Brand,
                        Capacity = createPostDto.Capacity,
                        MeasuringUnit = createPostDto.MeasuringUnit,
                        Volt = createPostDto.Volt,
                        IsProductPost = true,
                        ProductCategoryId = productCategoryModel.Id,
                    };

                    if(productCategoryModel.Name=="Battery" && solarProduct.Volt==null)
                    {
                        transactionScope.Dispose();
                        return BadRequest("volt can not be null");
                    }

                    solarProduct.ImageUrl = fileService.UploadFile("SolarProductImages", createPostDto.ProductImageUrl);
                    //create product
                    solarProduct = await solarProductRepository.CreateAsync(solarProduct);
                    //create post
                    postModel.SolarProductId = solarProduct.Id;
                    postModel = await postRepository.CreateAsync(postModel);
                    var postDto = postModel.PostInfo();

                    transactionScope.Complete(); // Commit transaction
                    return Ok(postDto);
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
        // update post
        [HttpPut]
        [Route("Update/{postId:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int postId, [FromForm] UpdatePostDto updatePostDto)
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
                    var existingPost = await postRepository.GetByIdInfoAsync(postId);
                    if (existingPost == null)
                    {
                        transactionScope.Dispose();
                        return NotFound("post not found");
                    }
                    var postModel = updatePostDto.UpdatePost();

                    // CategoryNameExist
                    var solarProduct = new SolarProduct
                    {
                        Price = updatePostDto.Price,
                        Brand = updatePostDto.Brand,
                        Capacity = updatePostDto.Capacity,
                        MeasuringUnit = updatePostDto.MeasuringUnit,
                        Volt = updatePostDto.Volt,
                        IsProductPost = true,
                        ProductCategoryId = existingPost.SolarProduct.ProductCategoryId,
                    };

                    if (existingPost.SolarProduct.ProductCategory.Name == "Battery" && solarProduct.Volt == null)
                    {
                        transactionScope.Dispose();
                        return BadRequest("volt can not be null");
                    }

                    //update image
                    if (updatePostDto.ProductImageUrl == null)
                    {
                        solarProduct.ImageUrl = existingPost.SolarProduct.ImageUrl;
                    }
                    else
                    {
                        solarProduct.ImageUrl = fileService.UpdateFile("SolarProductImages", updatePostDto.ProductImageUrl, existingPost.SolarProduct.ImageUrl);

                    }
                    //update database
                    solarProduct = await solarProductRepository.UpdateAsync(existingPost.SolarProductId, solarProduct);
                    if (solarProduct == null)
                    {
                        transactionScope.Dispose();
                        return NotFound("product not found");
                    }
                    postModel.Active = false;
                    postModel.CreatedOn = DateTime.Now;
                    postModel = await postRepository.UpdateAsync(postId,postModel);
                    if (postModel == null)
                    {
                        transactionScope.Dispose();
                        return NotFound("post not found");
                    }
                    var postDto = postModel.PostInfo();
                    transactionScope.Complete(); // Commit transaction
                    return Ok(postDto);
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
        // approve post
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("ApprovePost/{id:int}")]
        public async Task<IActionResult> ApprovePost([FromRoute] int id)
        {
            var post = await postRepository.ApprovePostAsync(id);
            if (post == null) { return NotFound(); }

            var postDto = post.PostInfo();

            var isUsed = "Used";
            if (!post.IsUsed)
            {
                isUsed = "New";
            }

            var message = new Message
            {
                SentDate = DateTime.Now,
                PersonId = post.PersonId,
                Title = "Post on " + post.CreatedOn.ToString("dd/MM/yyyy 'at' hh:mm tt") + " has been Approved",
                Body = isUsed + " " + postDto.Capacity_Unit + " " + postDto.Brand + " " +
                postDto.CategoryName + " for " + postDto.PriceStr + " in " + postDto.CompositeLocation,
            };

            message = await messageRepository.CreateAsync(message);

            // send email
            EmailDto emailDto = new EmailDto
            {
                To = post.Person.Account.Email,
                Subject = "Post Approval",
                Body = "Post on " + post.CreatedOn.ToString("dd/MM/yyyy 'at' hh:mm tt") + " has been Approved"
                +"\n"+ isUsed + " " + postDto.Capacity_Unit + " " + postDto.Brand + " " +
                postDto.CategoryName + " for " + postDto.PriceStr + " in " + postDto.CompositeLocation,
            };
            emailService.SendEmail(emailDto);

            return Ok("Approved Successfully");
        }

        // delete post
        [HttpDelete]
        [Route("RejectPost/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectPost([FromRoute] int id)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var postDomainModel = await postRepository.CheckRejectPostAsync(id);
                    if (postDomainModel == null) { return NotFound("post not found"); }

                    var post = await postRepository.DeleteAsync(id);

                    if (post == null) { return NotFound("post not found"); }
                    
                    var solarProduct = await solarProductRepository.DeleteAsync(post.SolarProductId);
                    if (solarProduct == null)
                    {
                        return NotFound("product not found");
                    }
                    fileService.DeleteFile(solarProduct.ImageUrl);

                    var postDto = post.PostInfo();

                    var isUsed = "Used";
                    if (!post.IsUsed)
                    {
                        isUsed = "New";
                    }

                    var message = new Message
                    {
                        SentDate = DateTime.Now,
                        PersonId = post.PersonId,
                        Title = "Post on " + post.CreatedOn.ToString("dd/MM/yyyy 'at' hh:mm tt") + " has been Rejected",
                        Body = isUsed + " " + postDto.Capacity_Unit + " " + postDto.Brand + " " +
                        postDto.CategoryName + " for " + postDto.PriceStr + " in " + postDto.CompositeLocation,
                    };

                    message = await messageRepository.CreateAsync(message);

                    // send email
                    EmailDto emailDto = new EmailDto
                    {
                        To = post.Person.Account.Email,
                        Subject = "Post Rejection",
                        Body = "Post on " + post.CreatedOn.ToString("dd/MM/yyyy 'at' hh:mm tt") + " has been Rejected"
                        + "\n" + isUsed + " " + postDto.Capacity_Unit + " " + postDto.Brand + " " +
                        postDto.CategoryName + " for " + postDto.PriceStr + " in " + postDto.CompositeLocation,
                    };
                    emailService.SendEmail(emailDto);

                    transactionScope.Complete(); // Commit transaction
                    return Ok("Post Rejected Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }

        //***************************************************************************************
        // delete post
        [HttpDelete]
        [Route("Delete/{postId:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int postId)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var existingPost = await postRepository.GetByIdInfoAsync(postId);
                    if (existingPost == null)
                    {
                        transactionScope.Dispose();
                        return NotFound("post not found");
                    }
                    var favoritePosts = await favoritePostRepository.DeleteAllByPostAsync(postId);

                    var postModel = await postRepository.DeleteAsync(postId);

                    if (postModel == null)
                    {
                        transactionScope.Dispose();
                        return NotFound("post not found");
                    }

                    var solarProduct = await solarProductRepository.DeleteAsync(postModel.SolarProductId);
                    if (solarProduct == null)
                    {
                        transactionScope.Dispose(); // Rollback transaction
                        return NotFound("product not found");
                    }
                    fileService.DeleteFile(solarProduct.ImageUrl);

                    transactionScope.Complete(); // Commit transaction
                    return Ok("Post Deleted Successfully");
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
