namespace FinanceSystem.Domain.FinanceAccounts.Enums;

public enum FinanceAccountStatus : byte
{
    Active = 1,
    CompletelyFrozen = 2,
    Closed = 3,
    InboundFrozen = 4,
    OutboundFrozen = 5
}
