using System.Globalization;

namespace MeawJson;

public static class JsonDeserializer
{
    public static T? Deserialize<T>(string json)
    {
        var parser = new JsonParser(json);

        object? parsedValue = parser.Parse();

        return (T?)ConvertValue(
            parsedValue,
            typeof(T));
    }

    private static object? ConvertValue(
        object? value,
        Type targetType)
    {

        if (value == null)
        {
            return null;
        }

        if (targetType.IsInstanceOfType(value))
        {
            return value;
        }

        if (targetType.IsPrimitive ||
            targetType == typeof(decimal))
        {
            return Convert.ChangeType(
                value,
                targetType,
                CultureInfo.InvariantCulture);
        }

        if (value is Dictionary<string, object?> dictionary)
        {
            return ConvertObject(dictionary, targetType);
        }

        if (value is List<object?> list)
        {
            return ConvertList(list, targetType);
        }

        return null;
    }
    private static object ConvertObject(
    Dictionary<string, object?> dictionary,
    Type targetType)
    {
        object instance = Activator.CreateInstance(targetType)!;

        var properties = targetType.GetProperties();

        foreach (var property in properties)
        {
            var matchingKey = dictionary.Keys.FirstOrDefault(
                key => string.Equals(
                    key,
                    property.Name,
                    StringComparison.OrdinalIgnoreCase));

            if (matchingKey == null)
            {
                continue;
            }

            object? propertyValue = dictionary[matchingKey];

            object? convertedValue = ConvertValue(
                propertyValue,
                property.PropertyType);

            property.SetValue(
                instance,
                convertedValue);
        }

        return instance;
    }
    private static object ConvertList(
    List<object?> list,
    Type targetType)
    {
        if (!targetType.IsGenericType ||
            targetType.GetGenericTypeDefinition() != typeof(List<>))
        {
            throw new JsonException(
                $"Cannot convert JSON array to {targetType.Name}.");
        }

        Type elementType =
            targetType.GetGenericArguments()[0];

        Type listType =
            typeof(List<>).MakeGenericType(elementType);

        object result =
            Activator.CreateInstance(listType)!;

        var addMethod =
            listType.GetMethod("Add")!;

        foreach (var item in list)
        {
            object? convertedItem =
                ConvertValue(item, elementType);

            addMethod.Invoke(
                result,
                new[] { convertedItem });
        }

        return result;
    }
}