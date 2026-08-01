namespace CodeHelper.Core.Interfaces;

public interface ICodeHelperAgent
{
    IAsyncEnumerable<string> RunAsync(string input, CancellationToken ct);
}