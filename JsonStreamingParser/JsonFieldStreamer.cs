using System.Text;

namespace JsonStreamingParser;

public sealed class JsonFieldStreamer
{
    private enum State
    {
        SearchingProperty,
        SearchingValue,
        ReadingString,
        ReadingArray,
        ReadingUnicodeEscape,
        Completed
    }

    private readonly string _propertyPattern;
    private readonly ConsoleColor _consoleColor;
    private readonly StringBuilder _searchBuffer = new();
    private readonly StringBuilder _unicodeBuffer = new(4);

    private State _state = State.SearchingProperty;

    private bool _escape;
    private bool _colorApplied;
    private bool _readingArrayItem;

    private ConsoleColor _previousColor;

    public JsonFieldStreamer(string propertyName, ConsoleColor consoleColor)
    {
        _propertyPattern = $"\"{propertyName.ToLowerInvariant()}\"";
        _consoleColor = consoleColor;
    }

    public bool IsCompleted => _state == State.Completed;

    public void ProcessChunk(string chunk)
    {
        if (IsCompleted || string.IsNullOrEmpty(chunk))
            return;

        foreach (char c in chunk)
        {
            switch (_state)
            {
                case State.SearchingProperty:
                    HandleSearchingProperty(c);
                    break;

                case State.SearchingValue:
                    HandleSearchingValue(c);
                    break;

                case State.ReadingString:
                    HandleReadingString(c);
                    break;

                case State.ReadingArray:
                    HandleReadingArray(c);
                    break;

                case State.ReadingUnicodeEscape:
                    HandleUnicodeEscape(c);
                    break;
            }
        }
    }

    private void HandleSearchingProperty(char c)
    {
        _searchBuffer.Append(c);

        if (_searchBuffer.Length > 128)
            _searchBuffer.Remove(0, _searchBuffer.Length - 64);

        if (_searchBuffer.ToString().EndsWith(_propertyPattern))
        {
            _state = State.SearchingValue;
            _searchBuffer.Clear();
        }
    }

    private void HandleSearchingValue(char c)
    {
        if (char.IsWhiteSpace(c) || c == ':')
            return;

        _state = c switch
        {
            '"' => State.ReadingString,
            '[' => State.ReadingArray,
            _ => _state
        };
    }

    private void HandleReadingString(char c)
    {
        BeginWrite();

        if (_escape)
        {
            HandleEscape(c);
            return;
        }

        switch (c)
        {
            case '\\':
                _escape = true;
                break;

            case '"':
                Complete();
                break;

            default:
                Console.Write(c);
                break;
        }
    }

    private void HandleReadingArray(char c)
    {
        if (_escape)
        {
            HandleEscape(c);
            return;
        }

        switch (c)
        {
            case '\\':
                _escape = true;
                break;

            case ']':
                Complete();
                break;

            case '"':
                if (!_readingArrayItem)
                {
                    BeginWrite();
                    Console.Write("- ");
                    _readingArrayItem = true;
                }
                else
                {
                    _readingArrayItem = false;
                    Console.WriteLine();
                }
                break;

            default:
                if (_readingArrayItem)
                    Console.Write(c);
                break;
        }
    }

    private void HandleEscape(char c)
    {
        _escape = false;

        if (c == 'u')
        {
            _unicodeBuffer.Clear();
            _state = State.ReadingUnicodeEscape;
            return;
        }

        Console.Write(Unescape(c));
    }

    private void HandleUnicodeEscape(char c)
    {
        _unicodeBuffer.Append(c);

        if (_unicodeBuffer.Length < 4)
            return;

        if (ushort.TryParse(
            _unicodeBuffer.ToString(),
            System.Globalization.NumberStyles.HexNumber,
            null,
            out var value))
        {
            Console.Write((char)value);
        }

        _unicodeBuffer.Clear();
        _state = State.ReadingString;
    }

    private void Complete()
    {
        EndWrite();
        _state = State.Completed;
        Console.WriteLine();
    }

    private void BeginWrite()
    {
        if (_colorApplied)
            return;

        _previousColor = Console.ForegroundColor;
        Console.ForegroundColor = _consoleColor;
        _colorApplied = true;
    }

    private void EndWrite()
    {
        if (!_colorApplied)
            return;

        Console.ForegroundColor = _previousColor;
        _colorApplied = false;
    }

    private static char Unescape(char c) => c switch
    {
        'b' => '\b',
        'f' => '\f',
        'n' => '\n',
        'r' => '\r',
        't' => '\t',
        '"' => '"',
        '\\' => '\\',
        '/' => '/',
        _ => c
    };
}