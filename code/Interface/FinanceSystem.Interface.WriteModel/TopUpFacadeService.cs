using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Domain.Contract.Payments;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;
using Shared.Core;
using Shared.Core.Exceptions;

namespace FinanceSystem.Interface.WriteModel;

public class TopUpFacadeService(
    ICommandBus commandBus,
    IEventListener listener,
    IUserResolver userResolver) : ITopUpFacadeService
{
    public async Task<JsonResponse<string>> Create(CreatePaymentModel model)
    {
        string externalReferenceId = string.Empty;
        await listener.Subscribe(new EventHandlerAction<PaymentCreatedEvent>(a =>
        {
            externalReferenceId = a.ExternalReferenceId;
        }));

        await commandBus.Dispatch(new CreatePaymentCommand
        {
            Purpose = model.Purpose,
            Channel = model.Channel,
            Amount = model.Amount,
            SourceFinanceAccountId = model.SourceFinanceAccountId,
            DestinationFinanceAccountId = model.DestinationFinanceAccountId,
            ExternalServiceId = model.ExternalServiceId,
            ExternalReferenceId = model.ExternalReferenceId,
            ExternalTag = model.ExternalTag,
            PspCode = model.PspCode
        });

        return JsonResponse<string>.Success(externalReferenceId);
    }

    public async Task<JsonResponse<string>> TopUpAsync(TopUpModel model, CancellationToken cancellationToken = default)
    {
        var ipgUrl = string.Empty;
        var tokenRequestFailed = false;

        await listener.Subscribe(new EventHandlerAction<PspTokenReceivedEvent>(a =>
        {
            ipgUrl = a.IpgUrl;
        }));

        await listener.Subscribe(new EventHandlerAction<PaymentTokenRequestFailedEvent>(_ =>
        {
            tokenRequestFailed = true;
        }));

        await commandBus.Dispatch(new TopUpCommand()
        {
            Amount = model.Amount,
            PspCode = model.PspCode,
            UserId = Guid.Parse(userResolver.GetUserId())
        }, cancellationToken);

        if (tokenRequestFailed)
        {
            return JsonResponse<string>.Failure("Payment gateway token request failed. Please try again.");
        }

        return JsonResponse<string>.Success(ipgUrl);
    }

    public async Task<JsonResponse<string>> CompleteTopUpProcess(string psp, IReadOnlyDictionary<string, string> fields, CancellationToken cancellationToken = default)
    {
        var outcome = TopUpOutcome.Failure;

        if (!Enum.TryParse<PspCode>(psp, ignoreCase: true, out var pspCode) || !Enum.IsDefined(pspCode))
        {
            return JsonResponse<string>.Failure(outcome.ToString().ToLowerInvariant());
        }

        await listener.Subscribe(new EventHandlerAction<TopUpCallbackHandledEvent>(a =>
        {
            outcome = a.Outcome;
        }));

        await commandBus.Dispatch(new TopUpCallbackCommand()
        {
            PspCode = pspCode,
            Fields = fields
        }, cancellationToken);

        return JsonResponse<string>.Success(outcome.ToString().ToLowerInvariant());
    }
}
