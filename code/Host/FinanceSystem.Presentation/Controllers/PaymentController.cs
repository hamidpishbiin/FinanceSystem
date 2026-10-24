using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;
using Shared.Core;

namespace FinanceSystem.Presentation.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController(IPaymentFacadeService facadeService) : ControllerBase
{
    [HttpPost("callback/{psp}")]
    public async Task<IActionResult> ProcessPaymentCallbackAsync(string psp, CancellationToken cancellationToken = default)
    {
        var form = await Request.ReadFormAsync(cancellationToken);

        var fields = form.ToDictionary(f => f.Key, f => f.Value.ToString(), StringComparer.OrdinalIgnoreCase);

        var response = await facadeService.ProcessPaymentCallbackAsync(psp, fields, cancellationToken);

        return response.Status ? Redirect(response.Data) : BadRequest(response);
    }
}
