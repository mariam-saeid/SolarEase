using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using SolarEase.Models.DTO.EnergyPredictionDto;
using Newtonsoft.Json;
using SolarEase.Models.Domain;
using System.Transactions;
using AutoMapper;
using SolarEase.Repositories;
using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SolarEase.Mappings;
using Microsoft.AspNetCore.Authorization;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HourlyEnergyPredictionController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly IHourlyAllSkyPredictionRepository hourlyAllSkyPredictionRepository;
        private readonly IPersonRepository personRepository;
        private readonly IMapper mapper;

        public HourlyEnergyPredictionController(IHourlyAllSkyPredictionRepository hourlyAllSkyPredictionRepository,
            IPersonRepository personRepository,IMapper mapper)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://solareasegp-hourly.hf.space"); // Update the URL accordingly
            this.hourlyAllSkyPredictionRepository = hourlyAllSkyPredictionRepository;
            this.personRepository = personRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        [Route("HourlyAllSky/{City}")]
        public async Task<IActionResult> GetHourlyAllSky([FromRoute] string City)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    HttpResponseMessage response = await _client.GetAsync("/api/" + City + "_hourly");

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();

                        var hourlyAllSkyPredictions = new List<HourlyAllSkyPrediction>();
                        var deletedHourlyEnergyPrediction = await hourlyAllSkyPredictionRepository.DeleteAllAsync(City);

                        JArray jsonArray = JArray.Parse(data);

                        var currentDate = string.Empty;
                        int counter = 0;

                        foreach (var item in jsonArray)
                        {
                            var dateString = item["date"].ToString();
                            var dateTime = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", null);
                            var day = dateTime.Day;

                            if (currentDate != day.ToString())
                            {
                                counter = counter+1;
                                currentDate = day.ToString();
                            }
                            var hourlyAllSkyPrediction = new HourlyAllSkyPrediction
                            {
                                DayNum = counter,
                                Date = dateTime.ToString("dddd"),
                                Hour = dateTime.ToString("h tt"),
                                City = City,
                                PredictedAllSky = Convert.ToDouble(item["PredictedAllSky"])
                            };

                            hourlyAllSkyPrediction = await hourlyAllSkyPredictionRepository.CreateAsync(hourlyAllSkyPrediction);
                            hourlyAllSkyPredictions.Add(hourlyAllSkyPrediction);
                        }
                        var hourlyEnergyPredictionsDtos = hourlyAllSkyPredictions.HourlyPredctionsInfo();

                        transactionScope.Complete();
                        return Ok(hourlyEnergyPredictionsDtos);
                    }
                    else
                    {
                        transactionScope.Dispose();
                        return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    return StatusCode(500, ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("GetHourlyPrediction/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> GetHourlyPrediction([FromRoute] int personId)
        {
            var person = await personRepository.GetByIdAsync(personId);
            if (person == null)
            {
                return NotFound("Person not found");
            }

            var hourlyAllSkyList = await hourlyAllSkyPredictionRepository.GetByCityAsync(person.City);

            var hourlyEnergyPredictionDtos = hourlyAllSkyList.HourlyPredctionsInfo();

            foreach (var hourlyEnergyPredictionDto in hourlyEnergyPredictionDtos)
            {
                hourlyEnergyPredictionDto.PredictedEnergy = Math.Round(hourlyEnergyPredictionDto.PredictedEnergy / 1000 * person.SystemSize * 0.75, 2);
                hourlyEnergyPredictionDto.PredictedEnergyStr = hourlyEnergyPredictionDto.PredictedEnergy.ToString() + " KW";
            }

            return Ok(hourlyEnergyPredictionDtos);
        }

        [HttpGet]
        [Route("GetHourlyPredictionByDay/{personId:int}/{day:int}")]
        public async Task<IActionResult> GetHourlyPredictionByDay([FromRoute] int personId, [FromRoute] int day)
        {
            var person = await personRepository.GetByIdAsync(personId);
            if (person == null)
            {
                return NotFound("Person not found");
            }

            var hourlyAllSkyList = await hourlyAllSkyPredictionRepository.GetByDayNumAsync(day,person.City);


            var hourlyEnergyPredictionDtos = hourlyAllSkyList.HourlyPredctionsInfo();

            foreach (var hourlyEnergyPredictionDto in hourlyEnergyPredictionDtos)
            {
                hourlyEnergyPredictionDto.PredictedEnergy = Math.Round(hourlyEnergyPredictionDto.PredictedEnergy/1000 * person.SystemSize * 0.75, 2);
                hourlyEnergyPredictionDto.PredictedEnergyStr = hourlyEnergyPredictionDto.PredictedEnergy.ToString() + " KW";
            }

            return Ok(hourlyEnergyPredictionDtos);
        }

        [HttpDelete]
        [Route("{City}")]
        public async Task<IActionResult> DeleteHourlyAllSky([FromRoute] string City)
        {

            var deletedHourlyEnergyPrediction = await hourlyAllSkyPredictionRepository.DeleteAllAsync(City);
            if (deletedHourlyEnergyPrediction == null)
                return NotFound();

            return Ok("deleted");


        }


    }
}