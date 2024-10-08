using System.Collections.Concurrent;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleUtilities.Utilities.Json {
    public class GenericObjectConverter : JsonConverter<ConcurrentDictionary<string, object>> {
        public override ConcurrentDictionary<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var dictionary = new ConcurrentDictionary<string, object>();

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token");

            while (reader.Read()) {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Expected PropertyName token");

                string key = reader.GetString() ?? "key?";
                reader.Read();

                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException("Expected StartObject token for value");

                string typeName = "";
                object? value = null;

                while (reader.Read()) {
                    if (reader.TokenType == JsonTokenType.EndObject)
                        break;

                    if (reader.TokenType == JsonTokenType.PropertyName) {
                        string propertyName = reader.GetString() ?? "Property?";
                        reader.Read();

                        if (propertyName == "$Type") {
                            typeName = reader.GetString() ?? "TypeName?";
                        }
                        else if (propertyName == "$Value") {
                            Type type = Type.GetType(typeName) ?? throw new Exception("Type not found");
                            value = JsonSerializer.Deserialize(ref reader, type, options);
                        }
                    }
                }

                if (value == null) throw new JsonException("Value not found");

                dictionary.TryAdd(key, value);
            }

            return dictionary;
        }

        public override void Write(Utf8JsonWriter writer, ConcurrentDictionary<string, object> value, JsonSerializerOptions options) {
            writer.WriteStartObject();

            foreach (var kvp in value) {
                writer.WritePropertyName(kvp.Key);

                writer.WriteStartObject();
                writer.WriteString("$Type", kvp.Value.GetType().FullName);
                writer.WritePropertyName("$Value");

                JsonSerializer.Serialize(writer, kvp.Value, kvp.Value.GetType(), options);
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }    
}
