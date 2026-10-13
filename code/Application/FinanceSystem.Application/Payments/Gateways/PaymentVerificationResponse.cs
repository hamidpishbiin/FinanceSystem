namespace FinanceSystem.Application.Payments.Gateways;

public class PaymentVerificationResponse
{
    public bool IsSuccess { get; set; }
    public decimal? Amount { get; set; }
    public int Code { get; set; }
    public PspTransactionType TransactionType { get; set; }
}
