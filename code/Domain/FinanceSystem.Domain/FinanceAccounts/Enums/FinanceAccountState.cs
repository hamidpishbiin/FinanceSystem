namespace FinanceSystem.Domain.FinanceAccounts.Enums;

public enum FinanceAccountState : byte
{
    Active = 1,
    CompletelyFrozen = 2,
    InboundFrozen = 3,
    OutboundFrozen = 4
}
