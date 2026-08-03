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
        Completed
    }

    private readonly string _propertyPattern;
    private readonly ConsoleColor _consoleColor;
    private readonly StringBuilder _searchBuffer = new();

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

                case State.Completed:
                    return;
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

        switch (c)
        {
            case '"':
                _state = State.ReadingString;
                break;

            case '[':
                _state = State.ReadingArray;
                break;
        }
    }

    private void HandleReadingString(char c)
    {
        BeginWrite();

        if (_escape)
        {
            Console.Write(Unescape(c));
            _escape = false;
            return;
        }

        switch (c)
        {
            case '\\':
                _escape = true;
                return;

            case '"':
                EndWrite();
                _state = State.Completed;
                Console.WriteLine();
                return;

            default:
                Console.Write(c);
                return;
        }
    }

    private void HandleReadingArray(char c)
    {
        if (_escape)
        {
            Console.Write(Unescape(c));
            _escape = false;
            return;
        }

        switch (c)
        {
            case ']':
                EndWrite();
                _state = State.Completed;
                Console.WriteLine();
                return;

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
                return;

            case '\\':
                _escape = true;
                return;

            default:
                if (_readingArrayItem)
                    Console.Write(c);
                return;
        }
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
        'n' => '\n',
        'r' => '\r',
        't' => '\t',
        '"' => '"',
        '\\' => '\\',
        '/' => '/',
        _ => c
    };
}
