    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    

namespace SurfForecast.Models;

// These models are for deserializing the incoming JSON from the API which will be parsed and converted to a custom framework
    public partial class SwellIn
    {
        [JsonProperty("ts")]
        public List<long> Ts { get; set; }

        [JsonProperty("units")]
        public Units Units { get; set; }

        [JsonProperty("swell1_height-surface")]
        public List<double?> SwellHeight { get; set; }

        [JsonProperty("swell1_period-surface")]
        public List<double?> SwellPeriod { get; set; }
        
        [JsonProperty("swell1_direction-surface")]
        public List<double?> SwellDirection { get; set; }

        [JsonProperty("warning")]
        public string Warning { get; set; }
    }

    public partial class Units
    {
        [JsonProperty("swell1_height-surface")]
        public string SwellHeight { get; set; }

        [JsonProperty("swell1_period-surface")]
        public string SwellPeriod { get; set; }

        [JsonProperty("swell1_direction-surface")]
        public string SwellDirection { get; set; }
    }

