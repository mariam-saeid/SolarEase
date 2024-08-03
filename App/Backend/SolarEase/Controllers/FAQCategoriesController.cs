using AutoMapper;
using SolarEase.Services;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using SolarEase.Models.DTO.FAQCategoryDto;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQCategoriesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IFAQCategoryRepository fAQCategoryRepository;
        private readonly IFAQRepository fAQRepository;

        public FAQCategoriesController(IFAQCategoryRepository fAQCategoryRepository,IFAQRepository fAQRepository ,IMapper mapper)
        {
            this.fAQCategoryRepository = fAQCategoryRepository;
            this.fAQRepository = fAQRepository;
            this.mapper = mapper;
        }

        // categories info
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAll()
        {
            var fAQCategories = await fAQCategoryRepository.GetAllAsync();
            var fAQCategoryDTOs = mapper.Map<List<FAQCategoryDto>>(fAQCategories);
            return Ok(fAQCategoryDTOs);
        }

        //***************************************************************************************
        // category info
        [HttpGet("{name}")]
        public async Task<ActionResult> GetByName([FromRoute] string name)
        {
            var fAQCategory = await fAQCategoryRepository.GetByNameAsync(name);
            if (fAQCategory == null)
            {
                return NotFound();
            }
            var fAQCategoryDTO = mapper.Map<FAQCategoryDto>(fAQCategory);
            return Ok(fAQCategoryDTO);
        }

        // category info
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var fAQCategory = await fAQCategoryRepository.GetByIdAsync(id);
            if (fAQCategory == null)
            {
                return NotFound();
            }
            var fAQCategoryDTO = mapper.Map<FAQCategoryDto>(fAQCategory);
            return Ok(fAQCategoryDTO);
        }

        //***************************************************************************************
        // create category
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateFAQCategoryDto createFAQCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingFAQCategory = await fAQCategoryRepository.GetByNameAsync(createFAQCategoryDto.Name);
            if (existingFAQCategory != null)
            {
                return BadRequest("Category with this name already exists.");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var fAQCategory = new FAQCategory
            {
                Name = createFAQCategoryDto.Name,
                Order = createFAQCategoryDto.Order
            };
            fAQCategory = await fAQCategoryRepository.CreateAsync(fAQCategory);
            var fAQCategoryDto = mapper.Map<FAQCategoryDto>(fAQCategory);
            return Ok(fAQCategoryDto);
        }

        //***************************************************************************************
        // update category
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CreateFAQCategoryDto updateFAQCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingFAQCategory = await fAQCategoryRepository.GetByIdAsync(id);
            if (existingFAQCategory == null)
            {
                return NotFound();
            }
            if(existingFAQCategory.Name == updateFAQCategoryDto.Name)
            {
                return Ok(existingFAQCategory);
            }
            existingFAQCategory = await fAQCategoryRepository.GetByNameAsync(updateFAQCategoryDto.Name);
            if (existingFAQCategory != null)
            {
                return BadRequest("Category with this name already exists.");
            }

            var fAQCategory = new FAQCategory
            {
                Name = updateFAQCategoryDto.Name,
                Order = updateFAQCategoryDto.Order
            };

            existingFAQCategory = await fAQCategoryRepository.UpdateAsync(id, fAQCategory);

            var fAQCategoryDto = mapper.Map<FAQCategoryDto>(existingFAQCategory);
            return Ok(fAQCategoryDto);
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
                    var existingFAQCategory = await fAQCategoryRepository.GetByIdAsync(id);
                    if (existingFAQCategory == null)
                    {
                        transactionScope.Dispose(); // Rollback transaction
                        return NotFound("Category not found");
                    }
                    var fAQs = await fAQRepository.GetByCategoryNameAsync(existingFAQCategory.Name);
                    foreach (var fQA in fAQs)
                    {
                        var fQAModel = await fAQRepository.DeleteAsync(fQA.Id);
                        if (fQAModel == null)
                        {
                            transactionScope.Dispose(); // Rollback transaction
                            return NotFound("fAQ not found");
                        }
                    }
                    var deletedFAQCategory = await fAQCategoryRepository.DeleteAsync(id);
                    if (deletedFAQCategory == null)
                    {
                        transactionScope.Dispose(); // Rollback transaction
                        return NotFound("Category not found");
                    }
                    var fAQCategoryDTO = mapper.Map<FAQCategoryDto>(deletedFAQCategory);
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
