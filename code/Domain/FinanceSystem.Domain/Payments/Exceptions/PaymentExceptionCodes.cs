namespace FinanceSystem.Domain.Payments.Exceptions;

public static class PaymentExceptionCodes
{
    public const int InvalidPaymentAmount = 204;
    public const int InvalidServiceId = 205;
    public const int InvalidReferenceId = 206;
    public const int InvalidPaymentPurpose = 208;
    public const int InvalidPaymentChannel = 209;
    public const int MissingPspCode = 210;
    public const int UnexpectedPspCode = 211;
    public const int InvalidToken = 212;
    public const int InvalidPaymentState = 213;
    public const int InvalidRawStatus = 214;
    public const int PaymentNotFound = 216;
    public const int PspCodeMismatch = 217;
    public const int CallbackTokenMismatch = 218;
    public const int DuplicateReferenceNumber = 219;
    public const int PaymentReverseFailed = 220;
    public const int InvalidPaymentDirection = 221;
}
