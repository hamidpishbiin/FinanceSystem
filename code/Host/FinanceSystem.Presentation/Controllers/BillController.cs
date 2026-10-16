using FinanceSystem.Interface.Contracts.Bills;
using FinanceSystem.Interface.Contracts.Bills.Models;
using FinanceSystem.Interface.Contracts.Bills.Services;
using Shared.Core;

namespace FinanceSystem.Presentation.Controllers;

[Route("api/bill")]
[ApiController]
public class BillController(
    IBillFacadeService billFacadeService,
    IUserResolver userResolver) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateBill([FromBody] CreateBillModel model, CancellationToken cancellationToken)
    {
        var userId = userResolver.GetUserId();

        var response = await billFacadeService.CreateBill(model, userId, cancellationToken);

        return Ok(response);
    }
}
