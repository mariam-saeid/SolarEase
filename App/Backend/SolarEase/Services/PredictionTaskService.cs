using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SolarEase.Controllers;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SolarEase.Services
{
    public class PredictionTaskService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PredictionTaskService> _logger;

        public PredictionTaskService(IServiceProvider serviceProvider, ILogger<PredictionTaskService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PredictionTaskService is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation("PredictionTaskService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    _logger.LogInformation("Current time: {time}", now);

                    // Schedule for the next
                    var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 23, 0, 0, now.Kind);
                    _logger.LogInformation("Next scheduled run time: {time}", nextRunTime);

                    if (now > nextRunTime)
                    {
                        nextRunTime = nextRunTime.AddDays(1);
                    }

                    var delayTime = nextRunTime - now;
                    _logger.LogInformation("Delaying task for: {time}", delayTime);

                    await Task.Delay(delayTime, stoppingToken);

                    var governmentNames = new List<string> { "Alexandria", "Aswan", "Asyut", "Beheira", "Beni Suef", "Cairo", "Dakahlia", "Damietta", "Faiyum", "Gharbia", "Giza", "Ismailia", "Kafr el-Sheikh", "Marsa Matruh", "Minya", "Monufia", "New Valley", "North Sinai", "Port Said", "Qalyubia", "Qena", "Red Sea", "Sharqia", "Sohag", "South Sinai", "Suez", "Luxor" };


                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dailyController = scope.ServiceProvider.GetRequiredService<DailyEnergyPredictionController>();
                        var hourlyController = scope.ServiceProvider.GetRequiredService<HourlyEnergyPredictionController>();

                        foreach (var government in governmentNames)
                        {
                            var dailyResult = await dailyController.GetDailyAllSky(government);
                            var hourlyResult = await hourlyController.GetHourlyAllSky(government);

                            // Log the daily result
                            _logger.LogInformation("Scheduled task executed at: {time}, Daily Result: {result}", DateTime.Now, dailyResult);

                            if (dailyResult is Microsoft.AspNetCore.Mvc.ObjectResult dailyObjectResult)
                            {
                                _logger.LogInformation("Daily Result status code: {statusCode}", dailyObjectResult.StatusCode);

                                if (dailyObjectResult.Value != null)
                                {
                                    var dailyJsonData = JsonSerializer.Serialize(dailyObjectResult.Value);
                                    _logger.LogInformation("Daily Result data: {data}", dailyJsonData);
                                }
                                else
                                {
                                    _logger.LogWarning("Daily Result data is null.");
                                }
                            }
                            else
                            {
                                _logger.LogWarning("Daily Result is not an ObjectResult.");
                            }

                            // Log the hourly result
                            _logger.LogInformation("Scheduled task executed at: {time}, Hourly Result: {result}", DateTime.Now, hourlyResult);

                            if (hourlyResult is Microsoft.AspNetCore.Mvc.ObjectResult hourlyObjectResult)
                            {
                                _logger.LogInformation("Hourly Result status code: {statusCode}", hourlyObjectResult.StatusCode);

                                if (hourlyObjectResult.Value != null)
                                {
                                    var hourlyJsonData = JsonSerializer.Serialize(hourlyObjectResult.Value);
                                    _logger.LogInformation("Hourly Result data: {data}", hourlyJsonData);
                                }
                                else
                                {
                                    _logger.LogWarning("Hourly Result data is null.");
                                }
                            }
                            else
                            {
                                _logger.LogWarning("Hourly Result is not an ObjectResult.");
                            }
                        }

                    }
                }
                catch (TaskCanceledException)
                {
                    // This is expected during application shutdown or task cancellation
                    _logger.LogInformation("Task was canceled.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in task execution");
                }
            }

            _logger.LogInformation("PredictionTaskService has stopped.");
        }
    }
}