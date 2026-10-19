using FinanceSystem.Interface.Contracts.TopUps.Models;
using FinanceSystem.Interface.Contracts.TopUps.Services;

namespace FinanceSystem.Presentation.Controllers;

[Route("api/topup")]
[ApiController]
//[Authorize]
public class TopUpController(ITopUpFacadeService facadeService) : ControllerBase
{
    [HttpPost("request")]
    public async Task<JsonResponse<string>> TopUp(TopUpModel model, CancellationToken cancellationToken = default)
    {
        return await facadeService.TopUpAsync(model, cancellationToken);
    }

    [HttpPost("callback/{psp}")]
    public async Task<IActionResult> CompleteTopUpProcess(string psp, CancellationToken cancellationToken = default)
    {
        var form = await Request.ReadFormAsync(cancellationToken);

        var fields = form.ToDictionary(f => f.Key, f => f.Value.ToString(), StringComparer.OrdinalIgnoreCase);

        var response = await facadeService.CompleteTopUpProcess(psp, fields, cancellationToken);

        return response.Status ? Redirect(response.Data) : BadRequest(response);
    }
}
