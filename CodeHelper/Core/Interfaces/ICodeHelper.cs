namespace CodeHelper.Core.Interfaces;

public interface ICodeHelper
{
    IAsyncEnumerable<string> RunAsync(string input, CancellationToken ct);
}