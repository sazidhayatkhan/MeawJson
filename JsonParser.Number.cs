using System.Globalization;

namespace MeawJson;

public partial class JsonParser
{
    private object ParseNumber()
    {
        int start = _position;

        if (Peek() == '-')
        {
            Next();
        }

        if (Peek() == '0')
        {
            Next();

            if (char.IsDigit(Peek()))
            {
                throw new JsonException(
                    $"Invalid number at position {_position}.");
            }
        }
        else if (Peek() >= '1' && Peek() <= '9')
        {
            while (char.IsDigit(Peek()))
            {
                Next();
            }
        }
        else
        {
            throw new JsonException(
                $"Invalid number at position {_position}.");
        }

        if (Peek() == '.')
        {
            Next();

            if (!char.IsDigit(Peek()))
            {
                throw new JsonException(
                    $"Expected digit after decimal point at position {_position}.");
            }

            while (char.IsDigit(Peek()))
            {
                Next();
            }
        }

        if (Peek() == 'e' || Peek() == 'E')
        {
            Next();

            if (Peek() == '+' || Peek() == '-')
            {
                Next();
            }

            if (!char.IsDigit(Peek()))
            {
                throw new JsonException(
                    $"Expected digit in exponent at position {_position}.");
            }

            while (char.IsDigit(Peek()))
            {
                Next();
            }
        }

        string numberText = _json[start.._position];

        if (numberText.Contains('.') ||
            numberText.Contains('e') ||
            numberText.Contains('E'))
        {
            if (double.TryParse(
                    numberText,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out double doubleValue))
            {
                return doubleValue;
            }
        }
        else
        {
            if (long.TryParse(
                    numberText,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out long longValue))
            {
                return longValue;
            }
        }

        throw new JsonException(
            $"Invalid number '{numberText}'.");
    }
}
