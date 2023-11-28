    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.ComponentModel.DataAnnotations;
    

namespace SurfForecast.Models;

public class LocationSelectViewModel
{   
    public int locationId { get; set; }
    public string locationName { get; set; }
    
}