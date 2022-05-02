using Microsoft.AspNetCore.Mvc;
// routing service 
namespace API.Controllers;

[ApiController]  // class of type ApiContoller
[Route("[controller]")]   
// controller is a placeholder, it will be replaced by the first part of the controller name
// eg: WeatherForecast , url route name 

// controller is similar to express module in node, eg : app.get('xxx/xx', func(req, res){})


public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


    //
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }
    //controller endpoint, http get request
    [HttpGet(Name = "GetWeatherForecast")]

    // define call back function in get method
    public IEnumerable<WeatherForecast> Get()
    {   
        //return an array with weather forecast 
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
