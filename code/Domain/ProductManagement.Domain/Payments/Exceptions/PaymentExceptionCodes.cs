namespace ProductManagement.Domain.Payments.Exceptions
{
    public static class PaymentExceptionCodes
    {
        public const short NegativeMoneyAmountExceptionCode = 200;
        public const short InvalidCurrencyExceptionCode = 201;
        public const short IncoherentPaymentExceptionCode = 202;
    }
}
