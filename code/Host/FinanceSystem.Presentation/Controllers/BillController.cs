using FinanceSystem.Interface.Contracts.Bills;
using FinanceSystem.Interface.Contracts.Bills.Models;
using FinanceSystem.Interface.Contracts.Bills.Services;
using Microsoft.Extensions.Options;
using Shared.Core;

namespace FinanceSystem.Presentation.Controllers;

[Route("api/bills")]
[ApiController]
public class BillController(
    IBillFacadeService facadeService,
    IBillFacadeQuery facadeQuery,
    IUserResolver userResolver,
    IOptions<PaymentResultPageConfig> paymentResultPageConfig) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBill([FromBody] CreateBillRequest request, CancellationToken cancellationToken)
    {
        var response = await facadeService.CreateBill(request, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{billId}/payment-options")]
    public async Task<IActionResult> GetPaymentOptions([FromRoute] GetPaymentOptionsRequest request, CancellationToken cancellationToken)
    {
        var response = await facadeQuery.GetPaymentOptionsAsync(long.Parse(request.BillId), long.Parse(userResolver.GetUserId()), cancellationToken);

        return Ok(response);
    }

    [HttpPost("{billId}/pay/psp/{pspCode}")]
    public async Task<IActionResult> PayWithPspAsync([FromRoute] PspPaymentRequest request, CancellationToken cancellationToken = default)
    {
        var userId = long.Parse(userResolver.GetUserId());

        var response = await facadeService.PayWithPspAsync(request, userId, cancellationToken);

        return Ok(response);
    }

    [HttpPost("{billId:long}/pay/wallet/{financeAccountId:long}")]
    public async Task<IActionResult> PayWithWalletAsync(long billId, long financeAccountId, CancellationToken cancellationToken)
    {
        var userId = long.Parse(userResolver.GetUserId());

        var response = await facadeService.PayWithWalletAsync(billId, userId, financeAccountId, cancellationToken);

        var redirectUrl = response.Status
            ? paymentResultPageConfig.Value.SuccessUrl
            : paymentResultPageConfig.Value.FailureUrl;

        return Redirect(redirectUrl);
    }
}
