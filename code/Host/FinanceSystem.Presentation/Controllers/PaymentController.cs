using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;

namespace FinanceSystem.Presentation.Controllers;

[Route("api/payment")]
[ApiController]
//[Authorize]
public class PaymentController(IPaymentFacadeService facadeService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<JsonResponse<string>> Create(CreatePaymentModel model)
    {
        return await facadeService.Create(model);
    }
}
