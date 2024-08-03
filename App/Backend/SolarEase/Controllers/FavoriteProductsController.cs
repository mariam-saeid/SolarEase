using AutoMapper;
using SolarEase.Models.Domain;
using SolarEase.Models.DTO.FavoriteProductDto;
using Microsoft.AspNetCore.Authorization;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolarEase.Mappings;
using SolarEase.Models.DTO.PersonDto;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteProductsController : ControllerBase
    {
        private readonly IFavoriteProductRepository favoriteProductRepository;
        private readonly IPersonRepository personRepository;
        private readonly ISolarProductRepository solarProductRepository;
        private readonly IMapper mapper;

        public FavoriteProductsController(IFavoriteProductRepository favoriteProductRepository,IPersonRepository personRepository,ISolarProductRepository solarProductRepository ,IMapper mapper)
        {
            this.favoriteProductRepository = favoriteProductRepository;
            this.personRepository = personRepository;
            this.solarProductRepository = solarProductRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var favoriteProducts = await favoriteProductRepository.GetAllAsync();
            if (favoriteProducts == null)
                return NotFound();
            
            var favoriteProductsDto = favoriteProducts.FavoriteProductsInfo();

            return Ok(favoriteProductsDto);
        }

        //***************************************************************************************
        [HttpGet]
        [Route("{personId:int}/{productId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int personId, [FromRoute] int productId)
        {
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var productModel = await solarProductRepository.GetByIdAsync(productId);
            if (productModel == null)
            {
                return NotFound("product not found");
            }
            var favoriteProduct = await favoriteProductRepository.GetByIdAsync(personId, productId);
            if (favoriteProduct == null)
                return NotFound();
            var favoriteProductDto = favoriteProduct.FavoriteProductInfo();

            return Ok(favoriteProductDto);
        }

        //***************************************************************************************
        [HttpGet]
        [Route("ByPerson/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> GetAllByPerson([FromRoute] int personId, String? query)
        {
            var solarProducts = await favoriteProductRepository.GetAllByPersonAsync(personId,query);
            if (solarProducts == null)
                NotFound("person not found");
            var solarProductDto = solarProducts.ProductsInfo();

            return Ok(solarProductDto);
        }

        [HttpGet]
        [Route("ByProduct/{productId:int}")]
        public async Task<IActionResult> GetAllByProductAsync([FromRoute] int productId)
        {
            var persons = await favoriteProductRepository.GetAllByProductAsync(productId);
            if (persons == null)
                return NotFound("product not found");
            var personsDto = persons.PersonsInfo();

            return Ok(personsDto);
        }

        //***************************************************************************************
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFavoriteProductDto createFavoriteProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personModel = await personRepository.GetByIdAsync(createFavoriteProductDto.PersonId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var productModel = await solarProductRepository.GetByIdAsync(createFavoriteProductDto.ProductId);
            if (productModel == null)
            {
                return NotFound("product not found");
            }
            var favoriteProductModel = await favoriteProductRepository.GetByIdAsync(createFavoriteProductDto.PersonId, createFavoriteProductDto.ProductId);
            if (favoriteProductModel != null)
            {
                return BadRequest("already favourite");
            }

            var favoriteProduct = mapper.Map<FavoriteProduct>(createFavoriteProductDto);
            favoriteProduct.AddedDate = DateTime.Now;
            favoriteProduct = await favoriteProductRepository.CreateAsync(favoriteProduct);
            var favoriteProductDto = favoriteProduct.FavoriteProductInfo();

            return Ok(favoriteProductDto);
        }

        //***************************************************************************************
        [HttpDelete]
        [Route("{personId:int}/{productId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int personId, [FromRoute] int productId)
        {
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var productModel = await solarProductRepository.GetByIdAsync(productId);
            if (productModel == null)
            {
                return NotFound("product not found");
            }
            var favoriteProduct = await favoriteProductRepository.DeleteAsync(personId, productId);
            if (favoriteProduct == null)
            {
                return NotFound("favorite product not found");

            }
            var favoriteProductDto = favoriteProduct.FavoriteProductInfo();

            return Ok(favoriteProductDto);

        }

        //***************************************************************************************
        // create favorite product
        [HttpPut]
        [Route("ToggleFavorite/{personId:int}/{productId:int}")]
        [Authorize]
        public async Task<IActionResult> ToggleFavorite([FromRoute] int personId, [FromRoute] int productId)
        {
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var productModel = await solarProductRepository.GetByIdAsync(productId);
            if (productModel == null)
            {
                return NotFound("product not found");
            }
            var favoriteProductModel = await favoriteProductRepository.GetByIdAsync(personId, productId);
            if (favoriteProductModel != null)
            {
                var favoriteProduct = await favoriteProductRepository.DeleteAsync(personId, productId);
            }
            else
            {
                var favoriteProduct = new FavoriteProduct
                {
                    PersonId = personId,
                    ProductId = productId,
                    AddedDate = DateTime.Now,
            };
                favoriteProduct = await favoriteProductRepository.CreateAsync(favoriteProduct);
            }

            return Ok("Updated Successfully");
        }


    }
}
