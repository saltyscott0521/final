using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SurfForecast.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using SurfForecast.DataAccess;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;


namespace SurfForecast.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public ApplicationDbContext dbContext;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        dbContext = context;
    }


    string key = "QagKTvhl3kEpGeCFVBFDsDkdYG60VuSy";
    string url = "https://api.windy.com/api/point-forecast/v2";


    

    public async Task<IActionResult> Index(int locationId = 1)
   
    {
      
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        
        string windData = "";
        WindIn? windin = null;
        string swellData = "";
        SwellIn? swellin = null;

        // Trunc tables to refresh with new forecast data
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Winds");
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Swells");  

        // Retrieve list of current locations from the database
        var locations = dbContext.Locations.ToList();
        // Pass locations to the view
        ViewBag.Locations = locations;
        foreach(var loc in locations){

        double lat = loc.Latitude;
        double lon = loc.Longitude;


        // Wind API Call
        
        Dictionary<string, object> param = new Dictionary<string, object>
                {
                { "lat", lat },
                { "lon", lon },
                { "model", "gfs" },
                { "parameters", new List<string> { "wind" } },
                { "key", key }
                };

        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(param);

        // Create a StringContent with the JSON data
        var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                                
        try
            {

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    windData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    
                    if (!windData.Equals(""))
                    {
                    // Deserialize the JSON response to a model
                    windin = JsonConvert.DeserializeObject<WindIn>(windData);
                    }
                }
            
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                //Console.WriteLine(windin.Ts[0]);
                //Console.WriteLine(windin.WindUSurface[0]);

                //Convert 2D wind vectors into one speed
                List<double> windSpeed = new List<double> {};
                List<DateTime> timestamp = new List<DateTime> {};
                for (int i = 0; i < windin.WindVSurface.Count; i++)
                {
                double ws = Math.Round(WindCalculator.CalculateWindSpeed(windin.WindUSurface[i], windin.WindVSurface[i]));
                windSpeed.Add(ws);
                DateTime t = DateTimeOffset.FromUnixTimeMilliseconds(windin.Ts[i]).DateTime;
                timestamp.Add(t);
                }
                // Convert Wind in to the Entity Framework we want in the database            
                
                // Loop through each timestamp and enter record into db
                for (int i=0; i<windin.Ts.Count; i++)
                {
                    Wind w = new Wind{Timestamp=timestamp[i],WindSpeed=windSpeed[i],LocationID=loc.ID};
                    dbContext.Winds.Add(w);
                }
                dbContext.SaveChanges();
                    
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
 
       // Swell API Call
        
        Dictionary<string, object> param_s = new Dictionary<string, object>
                {
                { "lat", lat },
                { "lon", lon },
                { "model", "gfsWave" },
                { "parameters", new List<string> { "swell1" } },
                { "key", key }
                };

        string jsonData_s = Newtonsoft.Json.JsonConvert.SerializeObject(param_s);

        // Create a StringContent with the JSON data
        var content_s = new StringContent(jsonData_s, System.Text.Encoding.UTF8, "application/json");

                                
        try
            {

                HttpResponseMessage response = await client.PostAsync(url, content_s);

                if (response.IsSuccessStatusCode)
                {
                    swellData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    
                    if (!swellData.Equals(""))
                    {
                    // Deserialize the JSON response to a model
                    swellin = JsonConvert.DeserializeObject<SwellIn>(swellData);
                    }
                }
            
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                Console.WriteLine(swellin.Ts[0]);
                Console.WriteLine(swellin.SwellHeight[0]);

                // Convert timestamp to DateTime
                List<DateTime> timestamp = new List<DateTime> {};
                for (int i = 0; i < swellin.SwellHeight.Count; i++)
                {
                DateTime t = DateTimeOffset.FromUnixTimeMilliseconds(swellin.Ts[i]).DateTime;
                timestamp.Add(t);
                }               
                
                // Loop through each timestamp and enter record into db
                for (int i=0; i<swellin.Ts.Count; i++)
                {
                    Swell s = new Swell{Timestamp=timestamp[i],SwellHeight=swellin.SwellHeight[i],LocationID=loc.ID};
                    dbContext.Swells.Add(s);
                }
                dbContext.SaveChanges();
                    
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
        }
            //Console.WriteLine("the location id is "+locationId);
            // Charts
            var WindChartLabels = dbContext.Winds.Where(w => w.LocationID == locationId).Select(w => w.Timestamp).ToList();
            var WindChartData = dbContext.Winds.Where(w => w.LocationID == locationId).Select(w => w.WindSpeed).ToList();

            WindChartModel WindModel = new WindChartModel
            {
                Labels = String.Join(",", WindChartLabels.Select(d => "'" + d + "'")),
                Data = String.Join(",", WindChartData.Select(d => d)),
            };     

            var SwellChartLabels = dbContext.Swells.Where(s => s.LocationID == locationId).Select(s => s.Timestamp).ToList();
            var SwellChartData = dbContext.Swells.Where(s => s.LocationID == locationId).Select(s => s.SwellHeight).ToList();

            SwellChartModel SwellModel = new SwellChartModel
            {
                Labels = String.Join(",", SwellChartLabels.Select(s => "'" + s + "'")),
                Data = String.Join(",", SwellChartData.Select(s => s)),
            };        

            string locationName = dbContext.Locations.Where(l => l.ID == locationId).Select(l => l.Name).First();

            LocationSelectViewModel locationSelectViewModel = new LocationSelectViewModel
            {
                locationId = locationId,
                locationName = locationName
            };
            
            ChartModel viewModel = new ChartModel
            {
                WindChartModel = WindModel,
                SwellChartModel = SwellModel,
                locationSelectViewModel = locationSelectViewModel
            };

            return View(viewModel);
        }
       
        
       public IActionResult About()
    {
        return View();
    }
       public IActionResult Location()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult AddLocation()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddLocation(LocationViewModel model)
    {
        if (ModelState.IsValid)
        {
            var newLocation = new Location
            {
                Name = model.Name,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };

            dbContext.Locations.Add(newLocation);
            dbContext.SaveChanges();

            // Redirect to the index or another page after successful submission
            return RedirectToAction("Index");
        }

        return View(model);
    }


}
    public class WindCalculator
    {
    public static double CalculateWindSpeed(double uComponent, double vComponent)
    {
        // Calculate wind speed using the formula: sqrt(U^2 + V^2)
        return Math.Sqrt(Math.Pow(uComponent, 2) + Math.Pow(vComponent, 2));
    }
    }

