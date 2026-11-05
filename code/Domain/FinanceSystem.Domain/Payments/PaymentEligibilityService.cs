using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using Shared.Core;

namespace FinanceSystem.Domain.Payments;

public class PaymentEligibilityService(IPaymentRepository paymentRepository) : IDomainService
{
    public async Task<Payment> GetPurchasePaymentAsync(long paymentId, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetByIdAsync(paymentId, cancellationToken);

        Guard<PaymentNotFoundException>.AgainstNull(payment);

        Guard<InvalidPaymentPurposeException>.IsTrue(payment!.Purpose != PaymentPurpose.Purchase);

        return payment;
    }

    public async Task ValidateReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken)
    {
        var isReferenceNumberUsed = await paymentRepository.GetByReferenceNumberAsync(referenceNumber, cancellationToken) is not null;

        Guard<DuplicateReferenceNumberException>.IsTrue(isReferenceNumberUsed);
    }
}
