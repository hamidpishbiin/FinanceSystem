using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;
using Shared.Core;

namespace FinanceSystem.Presentation.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController(
    IPaymentFacadeService facadeService,
    IPaymentFacadeQuery facadeQuery,
    IUserResolver userResolver) : ControllerBase
{
    [HttpGet("options")]
    public async Task<IActionResult> GetPaymentOptions([FromQuery] PaymentOptionsRequestModel model, CancellationToken cancellationToken)
    {
        model.UserId = long.Parse(userResolver.GetUserId());

        var response = await facadeQuery.GetPaymentOptionsAsync(model, cancellationToken);

        return Ok(response);
    }

    [HttpPost("pay-with-psp")]
    public async Task<IActionResult> PayWithPspAsync(PspPaymentModel model, CancellationToken cancellationToken = default)
    {
        var response = await facadeService.PayWithPspAsync(model, cancellationToken);

        return Ok(response);
    }

    [HttpPost("callback/{psp}")]
    public async Task<IActionResult> ProcessPaymentCallbackAsync(string psp, CancellationToken cancellationToken = default)
    {
        var form = await Request.ReadFormAsync(cancellationToken);

        var fields = form.ToDictionary(f => f.Key, f => f.Value.ToString(), StringComparer.OrdinalIgnoreCase);

        var response = await facadeService.ProcessPaymentCallbackAsync(psp, fields, cancellationToken);

        return response.Status ? Redirect(response.Data) : BadRequest(response);
    }

    [HttpPost("pay-with-wallet")]
    public async Task<IActionResult> PayWithWalletAsync(WalletPaymentModel model, CancellationToken cancellationToken)
    {
        model.UserId = long.Parse(userResolver.GetUserId());

        return Ok();
    }
}
