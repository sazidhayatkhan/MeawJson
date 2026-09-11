using System.Collections;
using System.Globalization;
using System.Collections.Generic;
namespace MeawJson;

public static class JsonSerializer
{
    public static string Serialize(object? value)
    {
        return Serialize(
            value,
            new HashSet<object>(
                ReferenceEqualityComparer.Instance));
    }

    private static string Serialize(
        object? value,
        HashSet<object> references)
    {
        if (value == null)
        {
            return "null";
        }

        switch (value)
        {
            case string: return $"\"{EscapeString((string)value)}\"";
            case bool: return (bool)value ? "true" : "false";
            case int or long or float or double or decimal:
                return Convert.ToString(value, CultureInfo.InvariantCulture)!;
            case DateTime dateTime:
                return $"\"{dateTime.ToString("O", CultureInfo.InvariantCulture)}\"";
            case Guid guid:
                return $"\"{guid}\"";
            case Enum e:
                return $"\"{e}\"";
            case Dictionary<string, object> dict:
                return SerializeDictionary(dict, references);
            case IEnumerable:
                return SerializeCollection((IEnumerable)value, references);
            default:
                return SerializeObject(value, references);
        }
    }

    private static string SerializeObject(
        object value,
        HashSet<object> references)
    {
        if (!references.Add(value))
        {
            throw new JsonException(
                $"Circular reference detected while serializing {value.GetType().Name}.");
        }

        try
        {
            var type = value.GetType();
            var properties = type.GetProperties();

            var parts = new List<string>();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(value);

                var jsonValue = Serialize(propertyValue, references);

                parts.Add($"\"{property.Name}\":{jsonValue}");
            }

            return "{" + string.Join(",", parts) + "}";
        }
        finally
        {
            references.Remove(value);
        }
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

    private static string SerializeCollection(
        IEnumerable collection,
        HashSet<object> references)
    {
        if (!references.Add(collection))
        {
            throw new JsonException(
                "Circular reference detected while serializing a collection.");
        }

        try
        {
            var parts = new List<string>();

            foreach (var item in collection)
            {
                parts.Add(Serialize(item, references));
            }

            return "[" + string.Join(",", parts) + "]";
        }
        finally
        {
            references.Remove(collection);
        }
    }

    private static string SerializeDictionary(
        Dictionary<string, object> dictionary,
        HashSet<object> references)
    {
        if (!references.Add(dictionary))
        {
            throw new JsonException(
                "Circular reference detected while serializing a dictionary.");
        }

        try
        {
            var parts = new List<string>();

            foreach (var pair in dictionary)
            {
                var key = EscapeString(pair.Key);

                var value = Serialize(pair.Value, references);

                parts.Add($"\"{key}\":{value}");
            }

            return "{" + string.Join(",", parts) + "}";
        }
        finally
        {
            references.Remove(dictionary);
        }
    }
}