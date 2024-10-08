using static SimpleUtilities.Threading.SimpleLock;

using File = SimpleUtilities.Utilities.Files.File;

namespace SimpleUtilities.Utilities.Configurations{
    public class ConfigManager{

        #region Variables

            private string configPath;
            private Dictionary<string, string> config;

            private char equalSymbol;

        #endregion

        #region Constructors

            public ConfigManager(string configPath){

                if (!System.IO.File.Exists(configPath)) throw new System.IO.FileNotFoundException("Config file not found");

                this.configPath = configPath;
                config = new Dictionary<string, string>();

                equalSymbol = '=';

                LoadConfig();
            }

        #endregion

        #region Methods

            private void LoadConfig(){
               try {
                    Lock(this);
                    var lines = File.Read(configPath);

                    foreach (var line in lines) {
                        var parts = line.Split(equalSymbol);
                        if (parts.Length != 2) continue;
                        config.Add(parts[0], parts[1]);
                    }
               }finally{ 
                   Unlock(this);
               }
            }

            public string? GetValue(string key){
                try {
                    Lock(this);
                    return config.GetValueOrDefault(key);
                }finally {
                    Unlock(this);
                }
            }

            public void SetValue(string key, string value){
                try {
                    Lock(this);
                    if (config.ContainsKey(key)) config[key] = value;
                    else config.Add(key, value);
                    SaveConfig();
                }
                finally {
                    Unlock(this);
                }
            }

            public void SaveConfig(){
               try { 
                   Lock(this);
                   var lines = new string[config.Count];
                   var i = 0;
                   foreach (var pair in config){
                       lines[i] = pair.Key + "=" + pair.Value;
                       i++;
                   }
                   File.Write(configPath, lines, false, false);
               }finally{ 
                   Unlock(this);
               }
            }

        #endregion
    }
}
