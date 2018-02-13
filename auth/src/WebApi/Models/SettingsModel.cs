using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Chiron.Auth.Models {
    public class SettingsModel {
        public string Deployment { get; set; }
        public string App { get; set; }
        public string Config { get; set; }
        public Build Build { get; set; }
    }

    /// <summary>
    /// Build information
    /// </summary>
    public class Build {
        /// <summary>
        /// Build version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Build tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Build date
        /// </summary>
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
