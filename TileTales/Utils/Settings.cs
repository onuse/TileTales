using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    internal class Settings
    {
        public string ServerAdress { get; set; }
        public int ServerPort { get; set; }
        public string AccountUsername { get; set; }
        public string AccountPassword { get; set; }
        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public bool Fullscreen { get; set; }

        public Settings()
        {
            // Set default values for the variables
            ServerAdress = "127.0.0.1";
            ServerPort = 8080;
            AccountUsername = "admin";
            AccountPassword = "admin";
            WindowHeight = 900;
            WindowWidth = 1400;
            Fullscreen = false;
        }
    }
}
