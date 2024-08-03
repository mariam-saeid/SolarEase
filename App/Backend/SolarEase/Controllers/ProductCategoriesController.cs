using AutoMapper;
using SolarEase.Services;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolarEase.Models.DTO.ProductCategoryDto;
using System.Transactions;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly IMapper mapper;
        private readonly ISolarProductRepository solarProductRepository;
        private readonly IFavoriteProductRepository favoriteProductRepository;
        private readonly FileService fileService;

        public ProductCategoriesController(IProductCategoryRepository productCategoryRepository, IMapper mapper, FileService fileService,
            ISolarProductRepository solarProductRepository,IFavoriteProductRepository favoriteProductRepository)
        {
            this.productCategoryRepository = productCategoryRepository;
            this.mapper = mapper;
            this.solarProductRepository = solarProductRepository;
            this.favoriteProductRepository = favoriteProductRepository;
            this.fileService = fileService;
        }

        // categories info
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var productCategories = await productCategoryRepository.GetAllAsync();
            var productCategoryDTOs = mapper.Map<List<ProductCategoryDto>>(productCategories);
            return Ok(productCategoryDTOs);
        }

        //***************************************************************************************
        // category info
        [HttpGet("{name}")]
        public async Task<ActionResult> GetByName([FromRoute] string name)
        {
            var productCategory = await productCategoryRepository.GetByNameAsync(name);
            if (productCategory == null)
            {
                return NotFound();
            }
            var productCategoryDTO = mapper.Map<ProductCategoryDto>(productCategory);
            return Ok(productCategoryDTO);
        }

        // category info
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var productCategory = await productCategoryRepository.GetByIdAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            var productCategoryDTO = mapper.Map<ProductCategoryDto>(productCategory);
            return Ok(productCategoryDTO);
        }

        //***************************************************************************************
        // create category
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductCategoryDto createProductCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProductCategory = await productCategoryRepository.GetByNameAsync(createProductCategoryDto.Name);
            if (existingProductCategory != null)
            {
                return BadRequest("Category with this name already exists.");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var productCategory = new ProductCategory
            {
                Name = createProductCategoryDto.Name
            };
            productCategory = await productCategoryRepository.CreateAsync(productCategory);
            var productCategoryDto = mapper.Map<ProductCategoryDto>(productCategory);
            return Ok(productCategoryDto);
        }

        //***************************************************************************************
        // update category
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CreateProductCategoryDto updateProductCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProductCategory = await productCategoryRepository.GetByIdAsync(id);
            if (existingProductCategory == null)
            {
                return NotFound();
            }
            if(existingProductCategory.Name == updateProductCategoryDto.Name)
            {
                return Ok(existingProductCategory);
            }
            existingProductCategory = await productCategoryRepository.GetByNameAsync(updateProductCategoryDto.Name);
            if (existingProductCategory != null)
            {
                return BadRequest("Category with this name already exists.");
            }

            var productCategory = new ProductCategory
            {
                Name = updateProductCategoryDto.Name
            };

            existingProductCategory = await productCategoryRepository.UpdateAsync(id, productCategory);

            var productCategoryDto = mapper.Map<ProductCategoryDto>(existingProductCategory);
            return Ok(productCategoryDto);
        }

        //***************************************************************************************
        // delete category
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var existingProductCategory = await productCategoryRepository.GetByIdAsync(id);
                    if (existingProductCategory == null)
                    {
                        transactionScope.Dispose(); // Rollback transaction
                        return NotFound("Category not found");
                    }
                    var products = await solarProductRepository.GetByCategoryNameAsync(existingProductCategory.Name);
                    foreach (var product in products)
                    {
                        var favoriteProducts = await favoriteProductRepository.DeleteAllByProductAsync(product.Id);
                        var solarProductModel = await solarProductRepository.DeleteAsync(product.Id);
                        if (solarProductModel == null)
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return NotFound("product not found");
                        }
                        //remove from wwwroot
                        fileService.DeleteFile(solarProductModel.ImageUrl);
                    }
                    var deletedProductCategory = await productCategoryRepository.DeleteAsync(id);
                    if (deletedProductCategory == null)
                    {
                        transactionScope.Dispose(); // Rollback transaction
                        return NotFound("Category not found");
                    }
                    var productCategoryDTO = mapper.Map<ProductCategoryDto>(deletedProductCategory);
                    transactionScope.Complete(); // Commit transaction
                    return Ok("category deleted");
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    transactionScope.Dispose(); // Rollback transaction
                    return StatusCode(500, "An error occurred while processing your request.");
                }

            }
        }
    }
}
