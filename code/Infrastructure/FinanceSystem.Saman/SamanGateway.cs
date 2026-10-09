using System.Net.Http.Json;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.Payments.Enums;
using Microsoft.Extensions.Options;
using Shared.Core;

namespace FinanceSystem.Saman;

internal class SamanGateway : IPspGateway
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILoggerService _loggerService;
    private readonly PspSettings _settings;

    public SamanGateway(
        IOptions<PspOptions> pspOptions,
        IHttpClientFactory httpClientFactory,
        ILoggerService loggerService)
    {
        _settings = pspOptions.Value.Saman;
        _httpClientFactory = httpClientFactory;
        _loggerService = loggerService;
    }

    public PspCode Code => PspCode.Saman;

    public async Task<PspTokenResponse> RequestTokenAsync(PspTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = new
            {
                action = "token",
                redirectUrl = _settings.TopUpCallBackUrl,
                terminalId = _settings.TerminalId,
                amount = request.Amount,
                resNum = request.ReferenceNumber
            };

            var httpClient = _httpClientFactory.CreateClient(nameof(PspCode.Saman));

            var apiResponse = await httpClient.PostAsJsonAsync("/OnlinePG/OnlinePG", payload, cancellationToken);

            apiResponse.EnsureSuccessStatusCode();

            var samanResponse = await apiResponse.Content.ReadFromJsonAsync<SamanTokenResponse>(cancellationToken);

            if (samanResponse is null)
            {
                throw new InvalidOperationException("Token samanResponse was null.");
            }

            var isSamanResponseSuccess = samanResponse.Status == 1;

            return new PspTokenResponse()
            {
                IsSuccess = isSamanResponseSuccess,
                FailureReason = isSamanResponseSuccess
                    ? null
                    : MapFailureReason(samanResponse.ErrorCode),
                PaymentToken = samanResponse.Token,
                IpgUrl = $"{_settings.BaseUrl}/OnlinePG/SendToken?token={samanResponse.Token}",
                RawStatus = samanResponse.Status.ToString(),
                RawStatusCode = samanResponse.ErrorCode,
                RawDescription = samanResponse.ErrorDesc
            };
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _loggerService.Error(ex, $"{Code} token request timed out after {_settings.TimeoutInSeconds} seconds");

            return MakeGenericFailedResponse(PspFailureReason.Timeout);
        }
        catch (HttpRequestException ex)
        {
            _loggerService.Error(ex, $"{Code} token request failed.");

            return MakeGenericFailedResponse(PspFailureReason.HttpException);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex, $"{Code} token request failed unexpectedly");

            return MakeGenericFailedResponse(PspFailureReason.Unknown);
        }
    }

    public async Task<PaymentVerificationResponse> VerifyPaymentAsync(PaymentVerificationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = new
            {
                refNum = request.ReferenceNumber,
                terminalNumber = _settings.TerminalId
            };

            var httpClient = _httpClientFactory.CreateClient(nameof(PspCode.Saman));

            var response = await httpClient.PostAsJsonAsync("/verifyTxnRandomSessionKey/ipg/VerifyTransaction", payload, cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadFromJsonAsync<SamanPaymentVerificationResponse>(cancellationToken);

            if (responseBody?.Success != true || responseBody.TransactionDetail is null)
            {
                // Reverse Payment
            }

            return new PaymentVerificationResponse()
            {
                PspTransactionType = PspTransactionType.Verify,
                IsSuccess = responseBody?.Success ?? false,
                Code = responseBody?.ResultCode?.ToString() ?? string.Empty,
                Description = responseBody?.ResultDescription ?? string.Empty,
                RawData = responseBody
            };
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _loggerService.Error(ex, $"{Code} verify request timed out after {_settings.TimeoutInSeconds} seconds");

            return MakeGenericFailedVerificationResponse();
        }
        catch (HttpRequestException ex)
        {
            _loggerService.Error(ex, $"{Code} verify request failed.");

            return MakeGenericFailedVerificationResponse();
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex, $"{Code} verify request failed unexpectedly");

            return MakeGenericFailedVerificationResponse();
        }
    }

    private static PspFailureReason MapFailureReason(int samanStatus)
    {
        return samanStatus switch
        {
            1 => PspFailureReason.None,
            -1 => PspFailureReason.InvalidTerminal,
            -3 => PspFailureReason.InvalidAmount,
            -4 => PspFailureReason.DuplicateReference,
            _ => PspFailureReason.Unknown
        };
    }

    private PspTokenResponse MakeGenericFailedResponse(PspFailureReason failureReason)
    {
        return new PspTokenResponse()
        {
            IsSuccess = false,
            FailureReason = failureReason
        };
    }

    private static PaymentVerificationResponse MakeGenericFailedVerificationResponse()
    {
        return new PaymentVerificationResponse()
        {
            IsSuccess = false,
            PspTransactionType = PspTransactionType.Verify
        };
    }
}
