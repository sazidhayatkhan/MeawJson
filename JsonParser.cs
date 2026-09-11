using System.Text;

namespace MeawJson;


public class JsonParser
{
    private readonly string _json;
    private int _position;

    public JsonParser(string json)
    {
        _json = json;
        _position = 0;
    }

    private char ParseEscapeSequence()
    {
        char character = Next();

        return character switch
        {
            '"' => '"',
            '\\' => '\\',
            '/' => '/',
            'b' => '\b',
            'f' => '\f',
            'n' => '\n',
            'r' => '\r',
            't' => '\t',

            'u' => ParseUnicodeEscape(),

            _ => throw new JsonException(
                $"Invalid escape sequence '\\{character}' at position {_position}.")
        };
    }

    private string ParseString()
    {
        if (Next() != '"')
        {
            throw new JsonException(
                $"Expected '\"' at position {_position}.");
        }

        var builder = new StringBuilder();

        while (true)
        {
            char character = Next();

            if (character == '\0')
            {
                throw new JsonException(
                    "Unterminated JSON string.");
            }

            if (character == '"')
            {
                return builder.ToString();
            }

            if (character == '\\')
            {
                builder.Append(ParseEscapeSequence());
                continue;
            }
            
            if (character < 0x20)
            {
                throw new JsonException(
                    $"Invalid control character in string at position {_position}.");
            }

            builder.Append(character);
        }
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

    private object? ParseValue()
    {
        SkipWhitespace();

        return Peek() switch
        {
            // '{' => ParseObject(),
            // '[' => ParseArray(),
            '"' => ParseString(),
            't' => ParseTrue(),
            'f' => ParseFalse(),
            'n' => ParseNull(),

            // '-' or >= '0' and <= '9'
            // => ParseNumber(),

            _ => throw new JsonException(
                $"Unexpected character '{Peek()}' at position {_position}.")
        };
    }

    private object? ParseNull()
    {
        Expect("null");
        return null;
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

    private char ParseUnicodeEscape()
    {
        int value = 0;

        for (int i = 0; i < 4; i++)
        {
            char character = Next();

            int digit = HexValue(character);

            if (digit == -1)
            {
                throw new JsonException(
                    $"Invalid Unicode escape at position {_position}.");
            }

            value = value * 16 + digit;
        }

        return (char)value;
    }

    private int HexValue(char character)
    {
        if (character >= '0' && character <= '9')
        {
            return character - '0';
        }

        if (character >= 'a' && character <= 'f')
        {
            return character - 'a' + 10;
        }

        if (character >= 'A' && character <= 'F')
        {
            return character - 'A' + 10;
        }

        return -1;
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
}