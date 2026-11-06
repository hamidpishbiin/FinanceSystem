using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.TopUps.Models;
using FinanceSystem.Interface.Contracts.TopUps.Services;
using Shared.Core;

namespace FinanceSystem.Interface.WriteModel;

public class TopUpFacadeService(
    ICommandBus commandBus,
    IEventListener listener,
    IUserResolver userResolver) : ITopUpFacadeService
{
    public async Task<JsonResponse<string>> Create(CreatePaymentModel model)
    {
        string referenceId = string.Empty;
        await listener.Subscribe(new EventHandlerAction<PaymentCreatedEvent>(a =>
        {
            referenceId = a.ReferenceId;
        }));

        await commandBus.Dispatch(new CreatePaymentCommand
        {
            Purpose = model.Purpose,
            Channel = model.Channel,
            Amount = model.Amount,
            UserId = model.FinanceAccountId,
            Direction = model.Direction,
            ServiceId = model.ServiceId,
            ReferenceId = model.ReferenceId,
            PspCode = model.PspCode,
        });

        return JsonResponse<string>.Success(referenceId);
    }

    public async Task<JsonResponse<PspHandoffModel>> TopUpAsync(TopUpModel model, CancellationToken cancellationToken = default)
    {
        PspHandoffModel? handoff = null;
        var tokenRequestFailed = false;

        await listener.Subscribe(new EventHandlerAction<PspHandoffReadyEvent>(a =>
        {
            handoff = new PspHandoffModel
            {
                Url = a.Handoff.Url,
                Method = a.Handoff.Method,
                Fields = a.Handoff.Fields
            };
        }));

        await listener.Subscribe(new EventHandlerAction<PaymentTokenRequestFailedEvent>(_ =>
        {
            tokenRequestFailed = true;
        }));

        await commandBus.Dispatch(new TopUpCommand()
        {
            Amount = model.Amount,
            PspCode = model.PspCode,
            UserId = long.Parse(userResolver.GetUserId())
        }, cancellationToken);

        if (tokenRequestFailed || handoff is null)
        {
            return JsonResponse<PspHandoffModel>.Failure("Payment gateway token request failed. Please try again.");
        }

        return JsonResponse<PspHandoffModel>.Success(handoff);
    }

    public async Task<JsonResponse<string>> CompleteTopUpProcess(string psp, IReadOnlyDictionary<string, string> fields, CancellationToken cancellationToken = default)
    {
        var outcome = TopUpOutcome.Failure;

        if (!Enum.TryParse<PspCode>(psp, ignoreCase: true, out var pspCode) || !Enum.IsDefined(pspCode))
        {
            return JsonResponse<string>.Failure(outcome.ToString().ToLowerInvariant());
        }

        var resultPageUrl = string.Empty;

        await listener.Subscribe(new EventHandlerAction<TopUpCallbackHandledEvent>(a =>
        {
            outcome = a.Outcome;
            resultPageUrl = a.ResultPageUrl;
        }));

        await commandBus.Dispatch(new TopUpCallbackCommand()
        {
            PspCode = pspCode,
            Fields = fields
        }, cancellationToken);

        return JsonResponse<string>.Success($"{resultPageUrl}?status={outcome.ToString().ToLowerInvariant()}");
    }
}
