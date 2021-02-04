using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Authorizer.CrossCutting
{
    public abstract class CustomDeserializer<T> : JsonConverter<T> where T : class
    {
        public abstract T BuildObject(JObject jObject);

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            return BuildObject(jObject);

        }
    }
}
