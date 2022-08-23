using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemocracyBot.Integration.Weather;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemocracyBot.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IWeatherService weatherService, ILogger<WeatherForecastController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogError("aaaa");

            return Ok(await _weatherService.GetWeather());
        }
    }
}