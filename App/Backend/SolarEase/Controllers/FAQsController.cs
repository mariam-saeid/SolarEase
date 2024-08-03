using AutoMapper;
using SolarEase.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using SolarEase.Services;
using System.Transactions;
using SolarEase.Models.DTO.FAQDto;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQsController : ControllerBase
    {
        private readonly IFAQRepository fAQRepository;
        private readonly IFAQCategoryRepository fAQCategoryRepository;
        private readonly IMapper mapper;

        public FAQsController(IFAQRepository fAQRepository, IFAQCategoryRepository fAQCategoryRepository,IMapper mapper)
        { 
            this.fAQRepository = fAQRepository;
            this.fAQCategoryRepository = fAQCategoryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fAQsModel = await fAQRepository.GetAllAsync();
            var fAQsDto = fAQsModel.FAQsInfo();
            return Ok(fAQsDto);
        }

        //***************************************************************************************
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var fAQModel = await fAQRepository.GetByIdAsync(id);
            if (fAQModel == null)
            {
                return NotFound();
            }
            var fAQDto = fAQModel.FAQInfo();
            return Ok(fAQDto);
        }

        //***************************************************************************************
        [HttpGet]
        [Route("categoryName/{categoryName}")]
        public async Task<IActionResult> GetByCategoryName([FromRoute] string categoryName)
        {
            var existingCategory = await fAQCategoryRepository.GetByNameAsync(categoryName);
            if (existingCategory == null)
            {
                return BadRequest("category not exists.");
            }
            var fAQsModel = await fAQRepository.GetByCategoryNameAsync(categoryName);
            if (fAQsModel == null)
            {
                return NotFound();
            }
            var fAQsDto = fAQsModel.FAQsInfo();
            return Ok(fAQsDto);
        }

        [HttpGet]
        [Route("categoryId/{categoryId:int}")]
        [Authorize]
        public async Task<IActionResult> GetByCategoryId([FromRoute] int categoryId)
        {
            var existingCategory = await fAQCategoryRepository.GetByIdAsync(categoryId);
            if (existingCategory == null)
            {
                return BadRequest("category not exists.");
            }
            var fAQsModel = await fAQRepository.GetByCategoryIdAsync(categoryId);
            if (fAQsModel == null)
            {
                return NotFound();
            }
            var fAQsDto = fAQsModel.FAQsInfo();

            fAQsDto.Add(
                    new FAQDto
                    {

                        Id = 0,
                        Answer = "back",
                        Question = "back",
                        Active = true,
                        Order = 0,
                        FAQCategoryId = existingCategory.Id,
                        FAQCategoryName = existingCategory.Name
                    }
                    );

            return Ok(fAQsDto);
        }

        //***************************************************************************************
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateFAQDto createFAQDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the product category name already exists
            var existingCategory = await fAQCategoryRepository.GetByIdAsync(createFAQDto.FAQCategoryId);
            if (existingCategory == null)
            {
                return BadRequest("category not exists.");
            }

            var fAQModel = mapper.Map<FAQ>(createFAQDto);

            fAQModel = await fAQRepository.CreateAsync(fAQModel);

            var fAQDto = fAQModel.FAQInfo();
            return Ok(fAQDto);
        }

        //***************************************************************************************
        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CreateFAQDto updateFAQDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the product category name already exists
            var existingCategory = await fAQCategoryRepository.GetByIdAsync(updateFAQDto.FAQCategoryId);
            if (existingCategory == null)
            {
                return BadRequest("category not exists.");
            }

            var fAQModel = mapper.Map<FAQ>(updateFAQDto);

            //update database
            fAQModel = await fAQRepository.UpdateAsync(id, fAQModel);
            if (fAQModel == null)
            {
                return NotFound();
            }
            var fAQDto = fAQModel.FAQInfo();
            return Ok(fAQDto);
        }

        //***************************************************************************************
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var fAQModel = await fAQRepository.DeleteAsync(id);
            if (fAQModel == null)
            {
                return NotFound();
            }
            var fAQDto = fAQModel.FAQInfo();
            return Ok(fAQDto);
        }

    }
}
