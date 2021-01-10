using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Logger.Helpers;
using TestApi.Helpers;


namespace TestApi.Controllers
{
    public class WeatherForecastController : ApiController
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IStructuredLogHelper _logHelper;

        public WeatherForecastController(IStructuredLogHelper logHelper)
        {
            _logHelper = logHelper;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var correlationId = (new HttpUtils().GetCorrelationId(Request));
            _logHelper.LogInfo(rng.Next(), "", correlationId);
            try
            {
                throw new Exception("test exception");
            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex, correlationId);
            }

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
