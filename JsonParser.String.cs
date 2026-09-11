using System.Text;

namespace MeawJson;

public partial class JsonParser
{
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
}
