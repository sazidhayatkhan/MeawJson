using System.Collections;
using System.Globalization;
using System.Collections.Generic;
namespace MeawJson;

public static class JsonSerializer
{
    public static string Serialize(object? value)
    {
        if (value == null)
        {
            return "null";
        }

        return value switch
        {
            string => $"\"{EscapeString((string)value)}\"",

            bool => (bool)value ? "true" : "false",

            int or long or float or double or decimal
                => Convert.ToString(value, CultureInfo.InvariantCulture)!,

            Dictionary<string, object> dict
                => SerializeDictionary(dict),

            IEnumerable => SerializeCollection((IEnumerable)value),


            _ => SerializeObject(value)
        };
    }

    private static string SerializeObject(object value)
    {
        var type = value.GetType();
        var properties = type.GetProperties();

        var parts = new List<string>();

        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(value);

            var jsonValue = Serialize(propertyValue);

            parts.Add($"\"{property.Name}\":{jsonValue}");
        }

        return "{" + string.Join(",", parts) + "}";
    }

    private static string EscapeString(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }
    private static string SerializeCollection(IEnumerable collection)
    {
        var parts = new List<string>();

        foreach (var item in collection)
        {
            parts.Add(Serialize(item));
        }

        return "[" + string.Join(",", parts) + "]";
    }

    private static string SerializeDictionary(
        Dictionary<string, object> dictionary)
    {
        var parts = new List<string>();

        foreach (var pair in dictionary)
        {
            var key = EscapeString(pair.Key);

            var value = Serialize(pair.Value);

            parts.Add($"\"{key}\":{value}");
        }

        return "{" + string.Join(",", parts) + "}";
    }
}