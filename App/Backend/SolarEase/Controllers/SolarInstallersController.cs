using AutoMapper;
using SolarEase.Services;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolarEase.Models.DTO.SolarInstallerDto;
using OfficeOpenXml;
using System.IO;
using System.Net.Http;
using OfficeOpenXml.Packaging.Ionic.Zlib;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolarInstallersController : ControllerBase
    {
        private readonly ISolarInstallerRepository solarInstallerRepository;
        private readonly IPersonRepository personRepository;
        private readonly IMapper mapper;
        private readonly FileService fileService;
        private readonly GoogleMapsApiService googleMapsApiService;
        private readonly HttpClient _httpClient;

        public SolarInstallersController(ISolarInstallerRepository solarInstallerRepository, IPersonRepository personRepository,
            IMapper mapper, FileService fileService, GoogleMapsApiService googleMapsApiService)
        {
            _httpClient = new HttpClient();
            this.solarInstallerRepository = solarInstallerRepository;
            this.personRepository = personRepository;
            this.mapper = mapper;
            this.fileService = fileService;
            this.googleMapsApiService = googleMapsApiService;
        }

        // solarInstallers save Latitude and Longitude
        [HttpGet]
        [Route("SaveLocation")]
        public async Task<IActionResult> SaveLocation()
        {
            try
            {
                var solarInstallersModel = await solarInstallerRepository.GetAllQueryAsync(null, null);

                foreach (var installer in solarInstallersModel)
                {
                    var installerLocation = await googleMapsApiService.GetGeoLocation(_httpClient, installer.Address, installer.City);
                    installer.Latitude = installerLocation.Latitude;
                    installer.Longitude = installerLocation.Longitude;
                    //update database
                    var newInstaller = await solarInstallerRepository.UpdateAsync(installer.Id, installer);
                    if (newInstaller == null)
                    {
                        return NotFound();
                    }
                }

                var solarInstallersDto = mapper.Map<List<SolarInstallerDto>>(solarInstallersModel);

                return Ok(solarInstallersDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

        // solarInstallers info
        [HttpGet]
        [Route("Sorted/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> GetAllSorted([FromRoute] int personId, string? nameQuery, string? cityQuery)
        {
            try
            {
                var person = await personRepository.GetByIdAsync(personId);
                if (person == null)
                {
                    return NotFound("person not found");
                }
                 // Get user's location
                var userLocation = await googleMapsApiService.GetGeoLocation(_httpClient,person.Location, person.City);

                var solarInstallersModel = await solarInstallerRepository.GetAllQueryAsync(nameQuery, cityQuery);
                var solarInstallersDto = mapper.Map<List<SolarInstallerDto>>(solarInstallersModel);

                // Calculate distance for each solar installer
                foreach (var installer in solarInstallersDto)
                {
                    double distance = await googleMapsApiService.GetDistance(_httpClient,userLocation.Latitude, userLocation.Longitude, installer.Latitude, installer.Longitude);
                    installer.DistanceFromUser = distance;
                }

                // Sort by distance
                solarInstallersDto = solarInstallersDto.OrderBy(installer => installer.DistanceFromUser).ToList();

                return Ok(solarInstallersDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }
        // solarInstallers info
        [HttpGet]
        public async Task<IActionResult> GetAll(String? nameQuery, String? cityQuery)
        {
            var solarInstallersModel = await solarInstallerRepository.GetAllQueryAsync(nameQuery, cityQuery);
            var solarInstallersDto = mapper.Map<List<SolarInstallerDto>>(solarInstallersModel);   //dist-src
            return Ok(solarInstallersDto);
        }

        //***************************************************************************************
        // solarInstaller info
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var solarInstallerModel = await solarInstallerRepository.GetByIdAsync(id);
            if (solarInstallerModel == null)
            {
                return NotFound();
            }
            var solarInstallerDto = mapper.Map<SolarInstallerDto>(solarInstallerModel);
            return Ok(solarInstallerDto);
        }

        //***************************************************************************************
        // create solarInstaller
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] CreateSolarInstallerDto createSolarInstallerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var solarInstallerModel = mapper.Map<SolarInstaller>(createSolarInstallerDto);

            //update database
            solarInstallerModel = await solarInstallerRepository.CreateAsync(solarInstallerModel);

            //convert Domain Model to DTO to pass it to client
            var solarInstallerDto = mapper.Map<SolarInstallerDto>(solarInstallerModel);
            return Ok(solarInstallerDto);
        }

        //***************************************************************************************
        // update solarInstaller
        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CreateSolarInstallerDto updateSolarInstallerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var solarInstallerModel = mapper.Map<SolarInstaller>(updateSolarInstallerDto);

            //update database
            solarInstallerModel = await solarInstallerRepository.UpdateAsync(id,solarInstallerModel);
            if (solarInstallerModel == null)
            {
                return NotFound();
            }

            //convert Domain Model to DTO to pass it to client
            var solarInstallerDto = mapper.Map<SolarInstallerDto>(solarInstallerModel);
            return Ok(solarInstallerDto);
        }

        //***************************************************************************************
        // delete solarInstaller
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var solarInstallerModel = await solarInstallerRepository.DeleteAsync(id);

            if (solarInstallerModel == null) { return NotFound(); }

            return Ok("Solar Installer Deleted Successfully");
        }
    }
}
