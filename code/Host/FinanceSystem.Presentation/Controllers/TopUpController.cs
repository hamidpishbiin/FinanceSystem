using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;

namespace FinanceSystem.Presentation.Controllers;

[Route("api/payment")]
[ApiController]
//[Authorize]
public class TopUpController(IPaymentFacadeService facadeService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<JsonResponse<string>> Create(CreatePaymentModel model)
    {
        return await facadeService.Create(model);
    }

    [HttpPost("topUp")]
    public async Task<IActionResult> TopUp(TopUpModel model, CancellationToken cancellationToken = default)
    {
        var response = await facadeService.TopUpAsync(model, cancellationToken);

        return Redirect(response.Data);
    }
}
