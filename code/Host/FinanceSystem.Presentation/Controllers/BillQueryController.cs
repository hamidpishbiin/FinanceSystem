using FinanceSystem.Interface.Contracts.Bills;
using FinanceSystem.Interface.Contracts.Bills.Models;
using Shared.Core;

namespace FinanceSystem.Presentation.Controllers;

[Route("api/bills")]
[ApiController]
public class BillQueryController(
    IBillFacadeQuery facadeQuery,
    IUserResolver userResolver) : ControllerBase
{
    [HttpGet("{billId}/payment-options")]
    public async Task<IActionResult> GetPaymentOptions([FromRoute] GetPaymentOptionsRequest request, CancellationToken cancellationToken)
    {
        var response = await facadeQuery.GetPaymentOptionsAsync(long.Parse(request.BillId), long.Parse(userResolver.GetUserId()), cancellationToken);

        return Ok(response);
    }
}
