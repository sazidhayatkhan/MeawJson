using System.Globalization;
using System.Collections;
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


        Type? underlyingType =
            Nullable.GetUnderlyingType(targetType);

        if (underlyingType != null)
        {
            return ConvertValue(
                value,
                underlyingType);
        }

        if (targetType == typeof(DateTime))
        {
            if (value is not string stringValue)
            {
                throw new JsonException(
                    $"Expected string for DateTime.");
            }

            if (DateTime.TryParse(
                    stringValue,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind,
                    out DateTime dateTime))
            {
                return dateTime;
            }

            throw new JsonException(
                $"Invalid DateTime value '{stringValue}'.");
        }

        if (targetType == typeof(Guid))
        {
            if (value is not string stringValue)
            {
                throw new JsonException(
                    $"Expected string for Guid.");
            }

            if (Guid.TryParse(
                    stringValue,
                    out Guid guid))
            {
                return guid;
            }

            throw new JsonException(
                $"Invalid Guid value '{stringValue}'.");
        }

        if (targetType.IsEnum)
        {
            if (value is not string stringValue)
            {
                throw new JsonException(
                    $"Expected string for enum {targetType.Name}.");
            }

            if (Enum.TryParse(
                    targetType,
                    stringValue,
                    ignoreCase: true,
                    out object? enumValue))
            {
                return enumValue;
            }

            throw new JsonException(
                $"Invalid value '{stringValue}' for enum {targetType.Name}.");
        }


        if (targetType.IsPrimitive ||
            targetType == typeof(decimal))
        {
            try
            {
                return Convert.ChangeType(
                    value,
                    targetType,
                    CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new JsonException(
                    $"Cannot convert '{value}' to {targetType.Name}.");
            }
        }


        if (value is Dictionary<string, object?> dictionary)
        {
            if (targetType.IsGenericType &&
                targetType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return ConvertDictionary(
                    dictionary,
                    targetType);
            }

            return ConvertObject(
                dictionary,
                targetType);
        }


        if (value is List<object?> list)
        {
            if (targetType.IsArray)
            {
                return ConvertArray(
                    list,
                    targetType);
            }

            return ConvertList(
                list,
                targetType);
        }

        throw new JsonException(
            $"Cannot convert value to {targetType.Name}.");
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
        Type elementType;

        if (targetType.IsGenericType)
        {
            elementType =
                targetType.GetGenericArguments()[0];
        }
        else
        {
            throw new JsonException(
                $"Cannot determine collection element type for {targetType.Name}.");
        }

        Type listType =
            typeof(List<>).MakeGenericType(elementType);

        object result =
            Activator.CreateInstance(listType)!;

        var addMethod =
            listType.GetMethod("Add")!;

        foreach (var item in list)
        {
            object? convertedItem =
                ConvertValue(
                    item,
                    elementType);

            addMethod.Invoke(
                result,
                new[] { convertedItem });
        }

        return result;
    }
    private static object ConvertArray(
    List<object?> list,
    Type targetType)
    {
        Type elementType =
            targetType.GetElementType()!;

        Array result =
            Array.CreateInstance(
                elementType,
                list.Count);

        for (int i = 0; i < list.Count; i++)
        {
            object? convertedItem =
                ConvertValue(
                    list[i],
                    elementType);

            result.SetValue(
                convertedItem,
                i);
        }

        return result;
    }

    private static object ConvertDictionary(
    Dictionary<string, object?> source,
    Type targetType)
    {
        if (!targetType.IsGenericType ||
            targetType.GetGenericTypeDefinition() != typeof(Dictionary<,>))
        {
            throw new JsonException(
                $"Unsupported dictionary type: {targetType.Name}.");
        }

        Type[] genericArguments =
            targetType.GetGenericArguments();

        Type keyType = genericArguments[0];
        Type valueType = genericArguments[1];

        if (keyType != typeof(string))
        {
            throw new JsonException(
                "JSON object keys must map to string dictionary keys.");
        }

        Type dictionaryType =
            typeof(Dictionary<,>).MakeGenericType(
                keyType,
                valueType);

        object result =
            Activator.CreateInstance(dictionaryType)!;

        var addMethod =
            dictionaryType.GetMethod("Add")!;

        foreach (var pair in source)
        {
            object? convertedValue =
                ConvertValue(
                    pair.Value,
                    valueType);

            addMethod.Invoke(
                result,
                new object?[]
                {
                pair.Key,
                convertedValue
                });
        }

        return result;
    }
}