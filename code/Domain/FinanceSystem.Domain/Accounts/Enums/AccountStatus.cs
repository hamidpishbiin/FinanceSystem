namespace FinanceSystem.Domain.Accounts.Enums;

public enum AccountStatus : byte
{
    Active = 1,
    CompletelyFrozen = 2,
    Closed = 3,
    InboundFrozen = 4,
    OutboundFrozen = 5
}
