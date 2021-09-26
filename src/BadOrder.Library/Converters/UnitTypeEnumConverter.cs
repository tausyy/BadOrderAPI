using BadOrder.Library.Models.Items;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BadOrder.Library.Converters
{
    public class UnitTypeEnumConverter : JsonConverter<UnitTypes>
    {
        public override UnitTypes Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                var enumValue = reader.GetString();
                var isValid = tryString(typeToConvert, enumValue);
                return isValid
                    ? (UnitTypes)Enum.Parse(typeToConvert, enumValue)
                    : throw new JsonException(invalidValue(enumValue));
            }

            if (reader.TokenType is JsonTokenType.Number)
            {
                var isValidInt = reader.TryGetInt32(out var enumValue);
                if (!isValidInt)
                {
                    throw new JsonException(invalidValue(getRawTokenValue(ref reader)));
                }
                var isValid = Enum.IsDefined(typeToConvert, enumValue);
                return isValid
                    ? (UnitTypes)Enum.Parse(typeToConvert, enumValue.ToString())
                    : throw new JsonException(invalidValue(enumValue.ToString()));
            }

            throw new JsonException(invalidType(reader.TokenType.ToString()));
        }

        public override void Write(Utf8JsonWriter writer, UnitTypes value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());

        private static bool tryString(Type enumType, string value)
        {
            var isNumber = int.TryParse(value, out var intValue);
            return isNumber ? Enum.IsDefined(enumType, intValue) : Enum.IsDefined(enumType, value);
        }

        private static string getRawTokenValue(ref Utf8JsonReader reader) =>
            reader.HasValueSequence
            ? Encoding.UTF8.GetString(reader.ValueSequence)
            : Encoding.UTF8.GetString(reader.ValueSpan);

        private static string invalidValue(string value) =>
            $"enumError\nunitType\n{value}\nInvalid value for enum type";

        private static string invalidType(string value) =>
            $"enumError\nunitType\n{value}\nInvalid type for enum value";


    }
}
