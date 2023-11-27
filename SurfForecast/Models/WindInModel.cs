    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    

namespace SurfForecast.Models;

// These models are for deserializing the incoming JSON from the API which will be parsed and converted to a custom framework
    public partial class WindIn
    {
        [JsonProperty("ts")]
        public List<long> Ts { get; set; }

        [JsonProperty("units")]
        public Units Units { get; set; }

        [JsonProperty("wind_u-surface")]
        public List<double> WindUSurface { get; set; }

        [JsonProperty("wind_v-surface")]
        public List<double> WindVSurface { get; set; }

        [JsonProperty("warning")]
        public string Warning { get; set; }
    }

    public partial class Units
    {
        [JsonProperty("wind_u-surface")]
        public string WindUSurface { get; set; }

        [JsonProperty("wind_v-surface")]
        public string WindVSurface { get; set; }
    }

