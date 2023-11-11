using Newtonsoft.Json;
using System;

namespace TileTales.Utils {
    internal class UserSettings {
        public string ServerAdress { get; set; }
        public int ServerPort { get; set; }
        public string AccountUsername { get; set; }
        public string AccountPassword { get; set; }
        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public bool Fullscreen { get; set; }

        public UserSettings() {
            // Set default values for the variables
            ServerAdress = "127.0.0.1";
            ServerPort = 27020;
            AccountUsername = "admin";
            AccountPassword = "admin";
            WindowHeight = 900;
            WindowWidth = 1400;
            Fullscreen = false;
        }

        public override String ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
