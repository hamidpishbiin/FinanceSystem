namespace FinanceSystem.Domain.Payments.Exceptions;

public static class PaymentExceptionCodes
{
    public const int InvalidSourceFinanceAccountId = 201;
    public const int InvalidDestinationFinanceAccountId = 202;
    public const int SameSourceAndDestinationFinanceAccount = 203;
    public const int InvalidPaymentAmount = 204;
    public const int InvalidOriginServiceId = 205;
    public const int InvalidExternalReferenceId = 206;
    public const int InvalidExternalTag = 207;
    public const int InvalidPaymentPurpose = 208;
    public const int InvalidPaymentChannel = 209;
    public const int MissingPspCode = 210;
    public const int UnexpectedPspCode = 211;
    public const int InvalidToken = 212;
    public const int InvalidPaymentState = 213;
}
