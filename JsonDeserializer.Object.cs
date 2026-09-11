namespace MeawJson;

public static partial class JsonDeserializer
{
    private static object ConvertObject(
    Dictionary<string, object?> dictionary,
    Type targetType)
    {
        if (targetType.IsInterface ||
    targetType.IsAbstract)
        {
            throw new JsonException(
                $"Cannot create an instance of {targetType.Name}.");
        }

        object? instance;

        try
        {
            instance = Activator.CreateInstance(targetType);
        }
        catch (Exception)
        {
            throw new JsonException(
                $"Type {targetType.Name} must have a parameterless constructor.");
        }

        if (instance == null)
        {
            throw new JsonException(
                $"Could not create an instance of {targetType.Name}.");
        }

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
}