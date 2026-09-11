using System.Collections;

namespace MeawJson;

public static partial class JsonDeserializer
{
    private static object ConvertList(
    List<object?> list,
    Type targetType)
    {
        Type? elementType = GetCollectionElementType(targetType);

        if (elementType == null)
        {
            throw new JsonException(
                $"Unsupported collection type: {targetType.Name}.");
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
                new object?[]
                {
                convertedItem
                });
        }

        if (targetType.IsAssignableFrom(result.GetType()))
        {
            return result;
        }

        throw new JsonException(
            $"Cannot create collection of type {targetType.Name}.");
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

    private static Type? GetCollectionElementType(
    Type targetType)
    {
        if (targetType.IsArray)
        {
            return targetType.GetElementType();
        }

        if (!targetType.IsGenericType)
        {
            return null;
        }

        Type genericType =
            targetType.GetGenericTypeDefinition();

        if (genericType == typeof(List<>) ||
            genericType == typeof(IList<>) ||
            genericType == typeof(ICollection<>) ||
            genericType == typeof(IEnumerable<>))
        {
            return targetType.GetGenericArguments()[0];
        }

        return null;
    }
}