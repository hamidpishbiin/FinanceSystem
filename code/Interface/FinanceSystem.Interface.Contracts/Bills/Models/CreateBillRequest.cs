namespace FinanceSystem.Interface.Contracts.Bills.Models;

public record CreateBillRequest(long ServiceId, long ReferenceId, long UserId, decimal Amount);
