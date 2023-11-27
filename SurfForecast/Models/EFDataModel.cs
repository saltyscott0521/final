    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    

namespace SurfForecast.Models;

    public class Location
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<Wind> Winds { get; set; }
        public ICollection<Swell> Swells { get; set; }
    }

    public class Wind
    {
        public int ID { get; set; }
        public DateTime Timestamp { get; set; }
        public double WindSpeed { get; set; }

        public int LocationID { get; set; }
        public Location Location { get; set; }
        
    }

    public class Swell
    {
        public int ID { get; set; }
        public DateTime Timestamp { get; set; }
        public double? SwellHeight { get; set; }

        public int LocationID { get; set; }
        public Location Location { get; set; }
        
    }

