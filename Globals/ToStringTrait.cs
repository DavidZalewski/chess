using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Chess.Globals
{
    public static class ToStringTrait
    {
        public static List<Type> TypesToSkip { get; set; } = new List<Type>();

        public static string ToDetailedString(this object obj)
        {
            if (obj == null)
            {
                return "Object is null";
            }

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new SkippableTypesContractResolver(TypesToSkip),
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(obj, settings);
        }

        // Add types to skip serialization for
        public static void AddTypeToSkip(Type type)
        {
            if (!TypesToSkip.Contains(type))
            {
                TypesToSkip.Add(type);
            }
        }
    }

    public class SkippableTypesContractResolver : DefaultContractResolver
    {
        private readonly HashSet<Type> _typesToSkip;

        public SkippableTypesContractResolver(IEnumerable<Type> typesToSkip)
        {
            _typesToSkip = new HashSet<Type>(typesToSkip);
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            // Check if the type itself or any base type is in the skip list
            if (ShouldSkipType(objectType))
            {
                // Return an empty contract to prevent serialization
                JsonObjectContract emptyContract = CreateObjectContract(objectType);
                emptyContract.Converter = new SkippableTypeConverter();
                return emptyContract;
            }
            return base.CreateContract(objectType);
        }

        private bool ShouldSkipType(Type type)
        {
            // Check if the type or any of its base types are in the skip list
            while (type != null)
            {
                if (_typesToSkip.Contains(type))
                {
                    return true;
                }
                type = type.BaseType; // Move up the inheritance hierarchy
            }
            return false;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // Get all the properties, including non-public ones
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .Select(p => base.CreateProperty(p, memberSerialization))
                .Where(p => !p.PropertyName.EndsWith("k__BackingField"))  // Exclude backing fields
                .ToList();

            // Get fields too if needed (optional)
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .Select(f => base.CreateProperty(f, memberSerialization))
                .Where(p => !p.PropertyName.EndsWith("k__BackingField"))  // Exclude backing fields if you include fields
                .ToList();

            // Add fields to the property list (optional)
            props.AddRange(fields);

            // Ensure properties can be serialized, even if they are protected
            foreach (var prop in props)
            {
                prop.Writable = true;  // Ensures that even non-public fields can be written
                prop.Readable = true;  // Ensures that they can be read
            }

            return props;
        }
    }

    public class SkippableTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true; // Applies to any type
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Write a placeholder or skip writing for the skipped type
            //writer.WriteStartObject();
            //writer.WritePropertyName("SkippedType");
            //writer.WriteValue(value.GetType().Name);
            //writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Reading skipped types is not supported.");
        }
    }
}
