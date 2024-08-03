using AutoMapper;
using SolarEase.Mappings;
using SolarEase.Models.DTO.SolarProductDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using SolarEase.Services;
using System.Transactions;


namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolarProductsController : ControllerBase
    {
        private readonly ISolarProductRepository solarProductRepository;
        private readonly IPersonRepository personRepository;
        private readonly IFavoriteProductRepository favoriteProductRepository;
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly IMapper mapper;
        private readonly FileService fileService;

        public SolarProductsController(ISolarProductRepository solarProductRepository,IPersonRepository personRepository,
        IFavoriteProductRepository favoriteProductRepository, IMapper mapper,
            FileService fileService, IProductCategoryRepository productCategoryRepository)
        {
            this.solarProductRepository = solarProductRepository;
            this.personRepository = personRepository;
            this.favoriteProductRepository = favoriteProductRepository;
            this.mapper = mapper;
            this.fileService = fileService;
            this.productCategoryRepository = productCategoryRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("{personId:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int personId,String? query)
        {
            var existingPerson = await personRepository.GetByIdAsync(personId);
            if (existingPerson == null)
            {
                return NotFound("person not found");
            }
            var solarProductsModel = await solarProductRepository.GetAllAsync(query);
            var solarProductsDto = solarProductsModel.ProductsInfo();
            foreach (var solarProductDto in solarProductsDto)
            {
                var favoriteProductModel = await favoriteProductRepository.GetByIdAsync(personId, solarProductDto.Id);
                if (favoriteProductModel == null)
                {
                    solarProductDto.IsFavorite = false;
                }
                else
                {
                    solarProductDto.IsFavorite = true;
                }
            }

            return Ok(solarProductsDto);
        }

        //***************************************************************************************
        [HttpGet]
        [Route("Id/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var solarProductModel = await solarProductRepository.GetByIdAsync(id);
            if (solarProductModel == null)
            {
                return NotFound();
            }
            var solarProductDto = solarProductModel.ProductInfo();
            return Ok(solarProductDto);
        }

        //***************************************************************************************
        [HttpGet]
        [Route("{categoryName}")]
        public async Task<IActionResult> GetByCategoryName([FromRoute] string categoryName)
        {
            var existingCategory = await productCategoryRepository.GetByNameAsync(categoryName);
            if (existingCategory == null)
            {
                ModelState.AddModelError("ProductCategoryName", "Product category not exists.");
                return BadRequest(ModelState);
            }
            var solarProducstModel = await solarProductRepository.GetByCategoryNameAsync(categoryName);
            if (solarProducstModel == null)
            {
                return NotFound();
            }
            var solarProductsDto = solarProducstModel.ProductsInfo();
            return Ok(solarProductsDto);
        }

        //***************************************************************************************
        [HttpGet]
        [Route("capacitySorted/{categoryName}/{minCapacity}/{maxCapacity}")]
        public async Task<IActionResult> GetAllCapacitySorted([FromRoute] string categoryName, [FromRoute] double minCapacity, [FromRoute] double maxCapacity)
        {
            var solarProductsModel = await solarProductRepository.GetAllCapacitySortedAsync(categoryName, minCapacity, maxCapacity);
            var solarProductsDto = solarProductsModel.ProductsInfo();
            return Ok(solarProductsDto);
        }

        //***************************************************************************************
        [HttpGet]
        [Route("avgPrice/{capacity:double}/{categoryName}")]
        public async Task<IActionResult> GetAvgPriceByCapacity([FromRoute] double capacity, [FromRoute] string categoryName, string? brandName)
        {
            var solarProductsAvgProce = await solarProductRepository.GetPriceByCapacityAsync(capacity, categoryName, brandName);
            return Ok(solarProductsAvgProce);
        }

        //***************************************************************************************
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateSolarProductDto createSolarProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the product category name already exists
            var existingCategory = await productCategoryRepository.GetByNameAsync(createSolarProductDto.CategoryName);
            if (existingCategory == null)
            {
                ModelState.AddModelError("ProductCategoryName", "Product category not exists.");
                return BadRequest(ModelState);
            }

            if (existingCategory.Name == "Battery" && createSolarProductDto.Volt == null)
            {
                return BadRequest("volt can not br null");
            }

            var solarProductModel = mapper.Map<SolarProduct>(createSolarProductDto);
            solarProductModel.ProductCategoryId = existingCategory.Id;
            solarProductModel.IsProductPost = false;
            solarProductModel.ImageUrl = fileService.UploadFile("SolarProductImages", createSolarProductDto.ImageUrl); //image handled to string;

            solarProductModel = await solarProductRepository.CreateAsync(solarProductModel);

            var solarProductDto = solarProductModel.ProductInfo();
            return Ok(solarProductDto);
        }

        //***************************************************************************************
        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CreateSolarProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Check category
            var existingCategory = await productCategoryRepository.GetByNameAsync(updateProductDto.CategoryName);
            if (existingCategory == null)
            {
                ModelState.AddModelError("ProductCategoryName", "Product category not exists.");
                return BadRequest(ModelState);
            }

            //Update all parameters
            var solarProductModel = mapper.Map<SolarProduct>(updateProductDto);
            solarProductModel.ProductCategoryId = existingCategory.Id;

            //update image and bool flag separately
            var old = await solarProductRepository.GetByIdAsync(id);
            if (old == null)
            {
                return NotFound();
            }
            solarProductModel.ImageUrl = fileService.UpdateFile("SolarProductImages", updateProductDto.ImageUrl, old.ImageUrl);
            
            //update database
            solarProductModel = await solarProductRepository.UpdateAsync(id, solarProductModel);
            if (solarProductModel == null)
            {
                return NotFound();
            }
            var solarProductDto = solarProductModel.ProductInfo();
            return Ok(solarProductDto);
        }

        //***************************************************************************************
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var favoriteProducts = await favoriteProductRepository.DeleteAllByProductAsync(id);
            var solarProductModel = await solarProductRepository.DeleteAsync(id);
            if (solarProductModel == null)
            {
                return NotFound();
            }
            //remove from wwwroot
            fileService.DeleteFile(solarProductModel.ImageUrl);
            var SolarProductDto = solarProductModel.ProductInfo();
            return Ok(SolarProductDto);
        }

    }
}
