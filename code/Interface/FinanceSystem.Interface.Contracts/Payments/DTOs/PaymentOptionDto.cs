namespace FinanceSystem.Interface.Contracts.Payments.DTOs;

public class PaymentOptionDto
{
    public required string Name { get; set; }
    public required int Channel { get; set; }
    public int? PspCode { get; set; }
    public long? FinanceAccountId { get; set; }
}
