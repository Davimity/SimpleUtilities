using SimpleUtilities.Threading;

using File = SimpleUtilities.Utilities.Files.File;

namespace SimpleUtilities.Utilities.Configurations{
    public class ConfigManager{

        #region Variables

            private string configPath;
            private Dictionary<string, string> config;

            private char equalSymbol;

            private object lockObject;

        #endregion

        #region Constructors

            public ConfigManager(string configPath){

                if (!System.IO.File.Exists(configPath)) throw new System.IO.FileNotFoundException("Config file not found");

                this.configPath = configPath;
                config = new Dictionary<string, string>();

                equalSymbol = '=';

                LoadConfig();

                lockObject = new object();
            }

        #endregion

        #region Methods

            private void LoadConfig(){
                using(new SimpleLock(lockObject)){
                    string[] lines = File.Read(configPath);

                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(equalSymbol);
                        if (parts.Length != 2) continue;
                        config.Add(parts[0], parts[1]);
                    }
                }
            }

            public string? GetValue(string key){
                using (new SimpleLock(lockObject)){
                    return config.ContainsKey(key) ? config[key] : null;
                }
            }

            public void SetValue(string key, string value){
                using (new SimpleLock(lockObject)){
                    if (config.ContainsKey(key)) config[key] = value;
                    else config.Add(key, value);

                    SaveConfig();
                }
            }

            public void SaveConfig(){
                using (new SimpleLock(lockObject)){
                    string[] lines = new string[config.Count];
                    int i = 0;
                    foreach (KeyValuePair<string, string> pair in config){
                        lines[i] = pair.Key + "=" + pair.Value;
                        i++;
                    }
                    File.Write(configPath, lines, false, false);
                }
            }

        #endregion
    }
}
