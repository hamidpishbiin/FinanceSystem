using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;

namespace FinanceSystem.Presentation.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController(IPaymentFacadeService facadeService) : ControllerBase
{
    [HttpPost("pay-with-psp")]
    public async Task<IActionResult> Pay(PspPaymentModel model, CancellationToken cancellationToken = default)
    {
        var response = await facadeService.PayWithPspAsync(model, cancellationToken);

        return Ok(response);
    }
}
