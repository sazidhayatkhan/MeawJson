namespace MeawJson;
public partial class JsonParser
{
    private List<object?> ParseArray()
{
    if (Next() != '[')
    {
        throw new JsonException(
            $"Expected '[' at position {_position}.");
    }

    var result = new List<object?>();

    SkipWhitespace();

    if (Peek() == ']')
    {
        Next();
        return result;
    }

    while (true)
    {
        SkipWhitespace();

        object? value = ParseValue();

        result.Add(value);

        SkipWhitespace();

        char next = Next();

        if (next == ']')
        {
            return result;
        }

        if (next != ',')
        {
            throw new JsonException(
                $"Expected ',' or ']' at position {_position}.");
        }
    }
}
}