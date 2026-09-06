namespace FinanceSystem.Domain.Payments.Exceptions;

public static class PaymentExceptionCodes
{
    public const int InvalidIdempotencyKey = 200;
    public const int InvalidSourceAccountId = 201;
    public const int InvalidDestinationAccountId = 202;
    public const int SameSourceAndDestinationAccount = 203;
    public const int InvalidPaymentAmount = 204;
    public const int InvalidOriginServiceId = 205;
    public const int InvalidExternalReferenceId = 206;
    public const int InvalidExternalTag = 207;
    public const int InvalidPaymentPurpose = 208;
    public const int InvalidPaymentChannel = 209;
    public const int MissingBankPaymentDetail = 210;
    public const int UnexpectedBankPaymentDetail = 211;
}
