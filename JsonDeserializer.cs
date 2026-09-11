using System.Globalization;

namespace MeawJson;

public static partial class JsonDeserializer
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
            if (IsNullableType(targetType))
            {
                return null;
            }

            throw new JsonException(
                $"Cannot assign null to {targetType.Name}.");
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
            $"Cannot convert JSON value of type " +
            $"{value.GetType().Name} to {targetType.Name}.");
    }

    private static bool IsNullableType(Type type)
    {
        return !type.IsValueType ||
               Nullable.GetUnderlyingType(type) != null;
    }
}