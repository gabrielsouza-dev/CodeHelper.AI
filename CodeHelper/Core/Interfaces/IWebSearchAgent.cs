namespace CodeHelper.Core.Interfaces;

public interface IWebSearchAgent
{
    Task<string?> RunAsync(string input, CancellationToken ct);
}