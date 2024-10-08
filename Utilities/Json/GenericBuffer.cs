using System.Collections.Concurrent;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleUtilities.Utilities.Json {
    ///<summary>Class that allows to store any type of data in a dictionary.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public class GenericBuffer {

        #region Variables

            private readonly ConcurrentDictionary<string, object> data;

        #endregion

        #region Constructors

            public GenericBuffer() {
                data = new ConcurrentDictionary<string, object>();
            }

            public GenericBuffer(ConcurrentDictionary<string, object> data) {
                this.data = data;
            }

        #endregion

        #region Public Methods

            /// <summary> Adds or updates a data in the dictionary. </summary>
            /// <param name="key">Data key.</param>
            /// <param name="value">Value to add or update.</param>
            public void AddData(string key, object value) {
                data.AddOrUpdate(key, value, (existingKey, existingValue) => value);
            }

            /// <summary>Obtains a data from the dictionary based on the given key.</summary>
            /// <param name="key">Key of the data to obtain.</param>
            /// <returns>Value associated with the key.</returns>
            public object GetData(string key) {
                data.TryGetValue(key, out object? value);

                if(value == null) throw new KeyNotFoundException("Key not found");
                return value;
            }

            /// <summary>Removes a data from the dictionary.</summary>
            /// <param name="key">Key of the data to remove.</param>
            /// <returns>True if the data was removed, false if it was not found.</returns>
            public bool RemoveData(string key) {
                return data.TryRemove(key, out _);
            }

            /// <summary>Verify if a key exists in the dictionary.</summary>
            /// <param name="key">Key to verify.</param>
            /// <returns>True if the key exists, false if it does not.</returns>
            public bool ContainsKey(string key) {
                return data.ContainsKey(key);
            }

            /// <summary>Obtain a read-only dictionary with all the key-value pairs.</summary>
            /// <returns>A read-only dictionary with all the key-value pairs.</returns>
            public IReadOnlyDictionary<string, object> GetAllData() {
                return new Dictionary<string, object>(data);
            }

            /// <summary>Serialize the current state to a JSON string.</summary>
            /// <returns>JSON string with the serialized data.</returns>
            public string SerializeToJson(bool prettyWriting = true) {
                var options = new JsonSerializerOptions{
                    WriteIndented = prettyWriting,
                    Converters = { new GenericObjectConverter() }
                };
                return JsonSerializer.Serialize(data, options);
            }

            ///<summary>Serialize the current state to a JSON file.</summary>
            ///<param name="path">Path to save the JSON file.</param>
            ///<param name="encrypt">True if the file should be encrypted, false otherwise.</param>
            ///<param name="key">Key to encrypt the file. ONLY IF ENCRYPT IS TRUE.</param>
            ///<param name="iv">IV to encrypt the file. ONLY IF ENCRYPT IS TRUE.</param>
            ///<param name="prettyWriting">True if the JSON should be written in a pretty format, false otherwise.</param>
            public void SerializeToJson(string path, bool encrypt = false, byte[]? key = null, byte[]? iv = null, bool prettyWriting = true, bool appendToFile = false) {
                string json = SerializeToJson(prettyWriting);
                SimpleUtilities.Utilities.Files.File.Write(path, [json], append: appendToFile, createIfNotExist: true, encrypt: encrypt, key: key, iv: iv);
            }

            /// <summary>Deserialize a JSON string to a GenericBuffer object.</summary>
            /// <param name="json">Content or path of the JSON file.</param>
            /// <param name="isPath">True if the json parameter is a path, false if it is the content.</param>
            /// <param name="isFileEncrypted">True if the file is encrypted, false otherwise. ONLY FOR PATHS.</param>
            /// <param name="key">Key to decrypt the file. ONLY FOR PATHS.</param>
            /// <param name="iv">IV to decrypt the file. ONLY FOR PATHS.</param>
            /// <returns>Instance of GenericBuffer with the deserialized data.</returns>
            public static GenericBuffer DeserializeFromJson(string json, bool isPath = false, bool isFileEncrypted = false, byte[]? key = null, byte[]? iv = null) {

                string content;

                if (isPath) {
                    
                    string[] lines = SimpleUtilities.Utilities.Files.File.Read(json, decrypt: isFileEncrypted, includeLineBreaks: true, key: key, iv: iv);
                    StringBuilder sb = new StringBuilder();
                    foreach (string line in lines) sb.Append(line);
                    content = sb.ToString();

                }else{
                    content = json;
                }

                var options = new JsonSerializerOptions{
                    Converters = { new GenericObjectConverter() }
                };

                ConcurrentDictionary<string, object>? data = JsonSerializer.Deserialize<ConcurrentDictionary<string, object>>(content, options);
               
                if (data == null) return new GenericBuffer();
                else return new GenericBuffer(data);
            }

        #endregion

    }
}
