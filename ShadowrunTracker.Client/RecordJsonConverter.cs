namespace ShadowrunTracker.Client
{
    using ShadowrunTracker.Data;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class RecordJsonConverter : JsonConverter<RecordBase>
    {
        private static readonly IReadOnlyDictionary<int, Type> _disciriminators;

        static RecordJsonConverter()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _disciriminators = 
                Assembly.GetAssembly(typeof(RecordBase))
                    .GetTypes()
                    .Where(t => typeof(RecordBase).IsAssignableFrom(t))
                    .Select(type => new { type, attr = type.GetCustomAttribute<TypeDiscriminatorAttribute>() })
                    .Where(tuple => tuple.attr != null)
                    .ToDictionary(a => a.attr.Discriminator, a => a.type);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }


        public override bool CanConvert(Type typeToConvert) => typeof(RecordBase).IsAssignableFrom(typeToConvert);

        public override RecordBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader readerClone = reader;

            if (readerClone.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string? propertyName = readerClone.GetString();
            if (propertyName != TypeDiscriminatorAttribute.JsonProperty)
            {
                throw new JsonException();
            }

            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            int typeDiscriminator = readerClone.GetInt32();
            RecordBase? record = null;
            if (_disciriminators.TryGetValue(typeDiscriminator, out var type))
            {
                record = (RecordBase?)JsonSerializer.Deserialize(ref reader, type);
            }
            else
            {
                throw new JsonException();
            }

            return record;
        }

        public override void Write(Utf8JsonWriter writer, RecordBase value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            var attribute = value.GetType().GetCustomAttribute<TypeDiscriminatorAttribute>();
            if (attribute == null)
            {
                throw new JsonException();
            }

            writer.WriteNumber(TypeDiscriminatorAttribute.JsonProperty, attribute.Discriminator);

            var doc = JsonSerializer.SerializeToDocument(value, value.GetType());
            foreach (var element in doc.RootElement.EnumerateObject())
            {
                element.WriteTo(writer);
            }

            writer.WriteEndObject();
        }
    }
}
