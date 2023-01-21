using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    internal class SettingsReader
    {
        private static string HARDCODED_PATH = "settings.json";
        private static SettingsReader _instance;
        private string _pathToSettingsFile;
        private Settings _settings;

        public SettingsReader(string pathToSettingsFile)
        {
            _pathToSettingsFile = pathToSettingsFile;
        }

        public static SettingsReader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SettingsReader(HARDCODED_PATH);
                }
                return _instance;
            }
        }

        public Settings GetSettings()
        {
            if (_settings != null)
            {
                return _settings;
            }

            return ReadSettingsFromPersistance();
        }

        public Settings ReadSettingsFromPersistance()
        {
            try
            {
                if (File.Exists(_pathToSettingsFile))
                {
                    string json = File.ReadAllText(_pathToSettingsFile);
                    _settings = JsonConvert.DeserializeObject<Settings>(json);
                } else
                {
                    _settings = new Settings();
                    SaveSettings();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error reading settings file: " + ex.Message);
            }

            return _settings;
        }

        public void SaveSettings()
        {
            var json = JsonConvert.SerializeObject(_settings);
            File.WriteAllText(_pathToSettingsFile, json);
        }
    }
}
