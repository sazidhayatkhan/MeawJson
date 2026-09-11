namespace MeawJson;

public partial class JsonParser
{
    private readonly string _json;
    private int _position;

    public JsonParser(string json)
    {
        _json = json;
        _position = 0;
    }

    public object? Parse()
    {
        var result = ParseValue();

        SkipWhitespace();

        if (_position != _json.Length)
        {
            throw new JsonException(
                $"Unexpected character '{Peek()}' at position {_position}.");
        }

        return result;
    }

    private char Peek()
    {
        if (_position >= _json.Length)
        {
            return '\0';
        }

        return _json[_position];
    }

    private char Next()
    {
        if (_position >= _json.Length)
        {
            return '\0';
        }

        return _json[_position++];
    }

    private void SkipWhitespace()
    {
        while (char.IsWhiteSpace(Peek()))
        {
            _position++;
        }
    }

    private void Expect(string expected)
    {
        foreach (char character in expected)
        {
            if (Next() != character)
            {
                throw new JsonException(
                    $"Expected '{expected}' at position {_position}.");
            }
        }
    }

    private object? ParseValue()
    {
        SkipWhitespace();

        return Peek() switch
        {
            '{' => ParseObject(),
            '[' => ParseArray(),
            '"' => ParseString(),
            't' => ParseTrue(),
            'f' => ParseFalse(),
            'n' => ParseNull(),

            '-' or >= '0' and <= '9'
            => ParseNumber(),

            _ => throw new JsonException(
                $"Unexpected character '{Peek()}' at position {_position}.")
        };
    }

    private bool ParseTrue()
    {
        Expect("true");
        return true;
    }

    private bool ParseFalse()
    {
        Expect("false");
        return false;
    }

    private object? ParseNull()
    {
        Expect("null");
        return null;
    }
}
