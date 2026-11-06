namespace FinanceSystem.Application.Contracts.Payments;

public sealed record PspHandoff(string Url, string Method, IReadOnlyDictionary<string, string> Fields);
