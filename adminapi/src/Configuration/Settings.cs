using System;

namespace Chiron.Admin.Models {

    public class Build {
        public string Version { get; set; }
        public DateTime Date { get; set; }
        public string Tag { get; set; }
    }

    public class Settings {
        public string Deployment { get; set; }
        public string App { get; set; }
        public string Config { get; set; }
        public Build Build { get; set; }
    }
}
