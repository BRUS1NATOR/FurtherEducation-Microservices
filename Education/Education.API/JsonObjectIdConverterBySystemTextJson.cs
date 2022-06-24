using MongoDB.Bson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Education.API
{
    public class JsonObjectIdConverterBySystemTextJson : JsonConverter<ObjectId>
    {
        public override ObjectId Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) 
        { 
            return new ObjectId(JsonSerializer.Deserialize<string>(ref reader, options)); 
        }

        public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
