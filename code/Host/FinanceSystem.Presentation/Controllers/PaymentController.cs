using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;

namespace FinanceSystem.Presentation.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController(IPaymentFacadeService facadeService) : ControllerBase
{
    [HttpPost("pay-with-psp")]
    public async Task<IActionResult> RequestPaymentTokenAsync(PspPaymentModel model, CancellationToken cancellationToken = default)
    {
        var response = await facadeService.RequestPaymentTokenAsync(model, cancellationToken);

        return Ok(response);
    }

    [HttpPost("callback/{psp}")]
    public async Task<IActionResult> CompletePaymentProcessAsync(string psp, CancellationToken cancellationToken = default)
    {
        var form = await Request.ReadFormAsync(cancellationToken);

        var fields = form.ToDictionary(f => f.Key, f => f.Value.ToString(), StringComparer.OrdinalIgnoreCase);

        var response = await facadeService.CompletePaymentProcessAsync(psp, fields, cancellationToken);

        return response.Status ? Redirect(response.Data) : BadRequest(response);
    }
}
