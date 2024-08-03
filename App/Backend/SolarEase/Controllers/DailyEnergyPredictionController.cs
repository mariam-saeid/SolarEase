using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Models.DTO.EnergyPredictionDto;
using SolarEase.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyEnergyPredictionController : ControllerBase
    {
        private readonly HttpClient _client;

        private readonly IDailyAllSkyPredictionRepository dailyAllSkyPredictionRepository;
        private readonly IMapper mapper;
        private readonly IPersonRepository personRepository;

        public DailyEnergyPredictionController(IDailyAllSkyPredictionRepository dailyAllSkyPredictionRepository, IMapper mapper,
            IPersonRepository personRepository)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://solareasegp-daily.hf.space"); // Update the URL accordingly
            this.dailyAllSkyPredictionRepository = dailyAllSkyPredictionRepository;
            this.mapper = mapper;
            this.personRepository = personRepository;
        }


        [HttpGet]
        [Route("DailyAllSky/{City}")]
        public async Task<IActionResult> GetDailyAllSky([FromRoute] string City)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    HttpResponseMessage response = await _client.GetAsync("/api/" + City + "_daily");

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();

                        var dailyAllSkyPredictions = new List<DailyAllSkyPrediction>();
                        var deletedDailyAllSkyPredictions = await dailyAllSkyPredictionRepository.DeleteAllAsync(City);

                        JArray jsonArray = JArray.Parse(data);
                        int i = 1;

                        foreach (var item in jsonArray)
                        {
                            var dateString = item["date"].ToString();
                            DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", null);

                            var dailyAllSkyPrediction = new DailyAllSkyPrediction
                            {
                                DayNum = i,
                                Date = date.ToString("dddd"),
                                City = City,
                                PredictedAllSky = Convert.ToDouble(item["PredictedAllSky"])
                            };
                            i = i + 1;

                            dailyAllSkyPrediction = await dailyAllSkyPredictionRepository.CreateAsync(dailyAllSkyPrediction);
                            dailyAllSkyPredictions.Add(dailyAllSkyPrediction);
                        }

                        var dailyEnergyPredictionDtos = dailyAllSkyPredictions.DailyPredctionsInfo();

                        transactionScope.Complete();
                        return Ok(dailyEnergyPredictionDtos);
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
        [Route("DailyPredictions/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> GetDailyPrediction([FromRoute] int personId)
        {
            var person = await personRepository.GetByIdAsync(personId);
            if (person == null)
            {
                return NotFound("Person not found");
            }

            var dailyAllSkyList = await dailyAllSkyPredictionRepository.GetLastFiveAsync(person.City);


            var dailyEnergyPredictionDtos = dailyAllSkyList.DailyPredctionsInfo();

            foreach (var dailyEnergyPredictionDto in dailyEnergyPredictionDtos)
            {
                dailyEnergyPredictionDto.PredictedEnergy = Math.Round(dailyEnergyPredictionDto.PredictedEnergy * person.SystemSize * 0.75, 2);
                dailyEnergyPredictionDto.PredictedEnergyStr = dailyEnergyPredictionDto.PredictedEnergy.ToString()+" KW";
            }

            return Ok(dailyEnergyPredictionDtos);
        }


        [HttpGet]
        [Route("OneDayPrediction/{personId:int}/{day:int}")]
        public async Task<IActionResult> GetOneDayPrediction([FromRoute] int personId, [FromRoute] int day)
        {
            var person = await personRepository.GetByIdAsync(personId);
            if (person == null)
            {
                return NotFound("Person not found");
            }

            var dailyAllSkyPrediction = await dailyAllSkyPredictionRepository.GetLastByDayNumAsync(day,person.City);
            if (dailyAllSkyPrediction == null)
            {
                return NotFound("day not found");
            }

            var dailyEnergyPredictionDto = dailyAllSkyPrediction.DailyPredctionInfo();

            dailyEnergyPredictionDto.PredictedEnergy = Math.Round(dailyEnergyPredictionDto.PredictedEnergy * person.SystemSize * 0.75,2);
            dailyEnergyPredictionDto.PredictedEnergyStr = dailyEnergyPredictionDto.PredictedEnergy.ToString() + " KW";

            return Ok(dailyEnergyPredictionDto);
        }

        [HttpGet]
        [Route("HighEnergy/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> GetHighEnergy([FromRoute] int personId)
        {
            var person = await personRepository.GetByIdAsync(personId);
            if (person == null)
            {
                return NotFound("Person not found");
            }

            var dailyAllSkyList = await dailyAllSkyPredictionRepository.GetLastFiveAsync(person.City);

            double maxPredictedEnergy = 0;

            foreach (var dailyAllSky in dailyAllSkyList)
            {
                var predictedEnergy = Math.Ceiling(dailyAllSky.PredictedAllSky * person.SystemSize * 0.75);
                if (predictedEnergy > maxPredictedEnergy)
                {
                    maxPredictedEnergy = predictedEnergy;
                }
            }

            maxPredictedEnergy = (int)maxPredictedEnergy + 5;

            return Ok(maxPredictedEnergy);
        }

        [HttpDelete]
        [Route("{City}")]
        public async Task<IActionResult> DeleteDailyAllSky([FromRoute] string City)
        {

            var deletedDailyEnergyPrediction = await dailyAllSkyPredictionRepository.DeleteAllAsync(City);
            if (deletedDailyEnergyPrediction == null)
                return NotFound();

            return Ok("deleted");


        }


    }
}