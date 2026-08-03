namespace CodeHelper.Core.Interfaces;

public interface ICodeHelperAgent
{
    IAsyncEnumerable<string> RunAsync(string input, string? webSearchResponse, CancellationToken ct);
}