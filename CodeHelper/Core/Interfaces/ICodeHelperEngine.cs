namespace CodeHelper.Core.Interfaces;

public interface ICodeHelperEngine
{
    IAsyncEnumerable<string> RunAsync(string input, CancellationToken ct);
}