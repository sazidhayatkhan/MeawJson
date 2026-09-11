namespace MeawJson;

public static partial class JsonDeserializer
{
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