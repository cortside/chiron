using System;

namespace Chiron.Registration.Customer.WebApi.Models {
    public class SettingsModel {
        public string Deployment { get; set; }
        public string App { get; set; }
        public string Config { get; set; }
        public Build Build { get; set; }
    }

    public class Build {
        public string Version { get; set; }
        public DateTime Date { get; set; }
    }
}
