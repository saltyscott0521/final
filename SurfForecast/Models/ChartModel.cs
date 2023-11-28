    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    

namespace SurfForecast.Models;

     public class ChartModel
    {
        public WindChartModel WindChartModel { get; set; }
        public SwellChartModel SwellChartModel { get; set; }
        public LocationSelectViewModel locationSelectViewModel { get; set; }
    }
    public class WindChartModel
    {
        public string Labels { get; set; }
        public string Data { get; set; }
    }
    public class SwellChartModel
    {
        public string Labels { get; set; }
        public string Data { get; set; }
    }