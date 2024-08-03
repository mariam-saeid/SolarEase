using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Models.DTO.PeakHourDto;
using SolarEase.Repositories;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeakHoursController : ControllerBase
    {
        private readonly IPeakHourRepository peakHourRepository;
        private readonly IMapper mapper;

        public PeakHoursController(IPeakHourRepository peakHourRepository,IMapper mapper)
        {
            this.peakHourRepository = peakHourRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var peakHoursModel = await peakHourRepository.GetAllAsync();
            var peakHoursDto = mapper.Map<List<PeakHourDto>>(peakHoursModel);
            return Ok(peakHoursDto);
        }
        
        //**************************************************************************************************************
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var peakHourModel = await peakHourRepository.GetByIdAsync(id);
            if (peakHourModel == null)
            {
                return NotFound();
            }
            var peakHourDto = mapper.Map<PeakHourDto>(peakHourModel);
            return Ok(peakHourDto);
        }
        
        //**************************************************************************************************************
        [HttpGet]
        [Route("{city}")]
        public async Task<IActionResult> GetByCity([FromRoute] string city)
        {
            var peakHourModel = await peakHourRepository.GetByCityAsync(city);
            if (peakHourModel == null)
            {
                return NotFound();
            }
            var peakHourDto = mapper.Map<PeakHourDto>(peakHourModel);
            return Ok(peakHourDto);
        }
        
        //**************************************************************************************************************
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePeakHourDto createPeakHourDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var peakHourModel = createPeakHourDto.PeakHourCreate();

            peakHourModel = await peakHourRepository.CreateAsync(peakHourModel);

            var peakHourDto = mapper.Map<PeakHourDto>(peakHourModel);
            return Ok(peakHourDto);
        }
        
        //**************************************************************************************************************
        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CreatePeakHourDto createPeakHourDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var peakHourModel = createPeakHourDto.PeakHourCreate();


            peakHourModel = await peakHourRepository.UpdateAsync(id, peakHourModel);
            if (peakHourModel == null)
            {
                return NotFound();
            }
            var peakHourDto = mapper.Map<PeakHourDto>(peakHourModel);
            return Ok(peakHourDto);
        }
        
        //**************************************************************************************************************
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var peakHourModel = await peakHourRepository.DeleteAsync(id);

            if (peakHourModel == null) { return NotFound(); }

            return Ok("Peak Hour Deleted Successfully");
        }
    }
}
