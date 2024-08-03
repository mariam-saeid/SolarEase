using AutoMapper;
using SolarEase.Services;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolarEase.Models.DTO.FavoritePostDto;
using System.Transactions;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritePostsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly FileService fileService;
        private readonly IFavoritePostRepository favoritePostRepository;
        private readonly IPersonRepository personRepository;
        private readonly IPostRepository postRepository;

        public FavoritePostsController(IMapper mapper,FileService fileService, IFavoritePostRepository favoritePostRepository,IPersonRepository personRepository, IPostRepository postRepository)
        {
            this.mapper = mapper;
            this.fileService = fileService;
            this.favoritePostRepository = favoritePostRepository;
            this.personRepository = personRepository;
            this.postRepository = postRepository;
        }

        // favorite posts info
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var favoritePostsModel = await favoritePostRepository.GetAllAsync();
            var favoritePostsDto = favoritePostsModel.FavoritePostsInfo();
            return Ok(favoritePostsDto);
        }

        // favorite posts for person
        [HttpGet]
        [Authorize]
        [Route("ByPerson/{personId:int}")]
        public async Task<IActionResult> GetAllByPerson([FromRoute] int personId, String? query)
        {
            var postsModel = await favoritePostRepository.GetAllByPersonAsync(personId, query);
            if (postsModel == null)
                return NotFound("person not found");
            var postsDto = postsModel.PostsInfo();
            return Ok(postsDto);
        }

        // small favorite posts for person
        [HttpGet]
        [Route("SmallPost/{personId:int}")]
        public async Task<IActionResult> GetAllByPersonSmallPost([FromRoute] int personId, String? query)
        {
            var postsModel = await favoritePostRepository.GetAllByPersonAsync(personId,query);
            if (postsModel == null)
                return NotFound("person not found");
            var postsDto = postsModel.SmallPostDtoInfo();
            return Ok(postsDto);
        }

        // persons for favorite post
        [HttpGet]
        [Route("ByPost/{postId:int}")]
        public async Task<IActionResult> GetAllByPost([FromRoute] int postId)
        {
            var personsModel = await favoritePostRepository.GetAllByPostAsync(postId);
            if (personsModel == null)
                return NotFound("post not found");
            var postsDto = personsModel.PersonsInfo();
            return Ok(postsDto);
        }

        //***************************************************************************************
        // favorite post info
        [HttpGet]
        [Route("{personId:int}/{postId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int personId,[FromRoute] int postId)
        {
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var postModel = await postRepository.GetByIdActiveInfoAsync(postId);
            if (postModel == null)
            {
                return NotFound("post not found");
            }
            var favoritePostModel = await favoritePostRepository.GetByIdAsync(personId,postId);
            if(favoritePostModel == null)
                return NotFound();

            var favoritePostsDto = favoritePostModel.FavoritePostInfo();
            return Ok(favoritePostsDto);
        }

        //***************************************************************************************
        // create favorite post
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] CreateFavoritePostDto createFavoritePostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var personModel = await personRepository.GetByIdAsync(createFavoritePostDto.PersonId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var postModel = await postRepository.GetByIdActiveInfoAsync(createFavoritePostDto.PostId);
            if (postModel == null)
            {
                return NotFound("post not found");
            }
            var favoritePostModel = await favoritePostRepository.GetByIdAsync(createFavoritePostDto.PersonId, createFavoritePostDto.PostId);
            if (favoritePostModel != null)
                return BadRequest("already favourite");

            var favoritePost = mapper.Map<FavoritePost>(createFavoritePostDto);   //dist-src

            favoritePost.AddedDate = DateTime.Now;

            favoritePost = await favoritePostRepository.CreateAsync(favoritePost);

            return Ok("favourited Successfully");
        }

        //***************************************************************************************
        // delete favorite post
        [HttpDelete]
        [Route("{personId:int}/{postId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int personId,[FromRoute] int postId)
        {
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var postModel = await postRepository.GetByIdActiveInfoAsync(postId);
            if (postModel == null)
            {
                return NotFound("post not found");
            }
            var favoritePostModel = await favoritePostRepository.DeleteAsync(personId, postId);
            if (favoritePostModel == null)
                return NotFound("favorite post not found");

            return Ok("favorite post Deleted Successfully");
        }

        //***************************************************************************************
        // create favorite post
        [HttpPut]
        [Route("ToggleFavorite/{personId:int}/{postId:int}")]
        [Authorize]
        public async Task<IActionResult> ToggleFavorite([FromRoute] int personId, [FromRoute] int postId)
        {
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var postModel = await postRepository.GetByIdActiveInfoAsync(postId);
            if (postModel == null)
            {
                return NotFound("post not found");
            }
            var favoritePostModel = await favoritePostRepository.GetByIdAsync(personId,postId);
            if (favoritePostModel != null)
            {
                var favoritePost = await favoritePostRepository.DeleteAsync(personId, postId);
            }
            else
            {
                var favoritePost = new FavoritePost
                {
                    PersonId = personId,
                    PostId = postId,
                    AddedDate = DateTime.Now,
                };
                favoritePost = await favoritePostRepository.CreateAsync(favoritePost);
            }

            return Ok("Updated Successfully");
        }
    }
}
