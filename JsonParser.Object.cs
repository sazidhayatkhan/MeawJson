namespace MeawJson;

public partial class JsonParser
{
    private Dictionary<string, object?> ParseObject()
    {
        if (Next() != '{')
        {
            throw new JsonException(
                $"Expected '{{' at position {_position}.");
        }

        var result = new Dictionary<string, object?>();

        SkipWhitespace();

        if (Peek() == '}')
        {
            Next();
            return result;
        }

        while (true)
        {
            SkipWhitespace();

            if (Peek() != '"')
            {
                throw new JsonException(
                    $"Expected property name at position {_position}.");
            }

            string propertyName = ParseString();

            SkipWhitespace();

            if (Next() != ':')
            {
                throw new JsonException(
                    $"Expected ':' after property name at position {_position}.");
            }

            SkipWhitespace();

            object? propertyValue = ParseValue();

            result[propertyName] = propertyValue;

            SkipWhitespace();

            char next = Next();

            if (next == '}')
            {
                return result;
            }

            if (next != ',')
            {
                throw new JsonException(
                    $"Expected ',' or '}}' at position {_position}.");
            }
        }
    }
}