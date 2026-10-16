namespace FinanceSystem.Interface.Contracts.Bills.Models;

public record CreateBillModel(long ServiceId, long ReferenceId, decimal Amount);
