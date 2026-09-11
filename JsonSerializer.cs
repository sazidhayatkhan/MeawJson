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
                => value.ToString()!,

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
}