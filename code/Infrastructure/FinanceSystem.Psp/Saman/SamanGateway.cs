using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.RequestsToPay.Enums;
using Microsoft.Extensions.Options;
using Shared.Core;

namespace FinanceSystem.Psp.Saman;

internal class SamanGateway : IPspGateway
{
    private readonly IOptionsMonitor<PspOptions> _pspOptions;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILoggerService _loggerService;

    public SamanGateway(
        IOptionsMonitor<PspOptions> pspOptions,
        IHttpClientFactory httpClientFactory,
        ILoggerService loggerService)
    {
        _pspOptions = pspOptions;
        _httpClientFactory = httpClientFactory;
        _loggerService = loggerService;
    }

    public PspCode Code => PspCode.Saman;

    private PspSettings Settings => _pspOptions.CurrentValue.Saman;

    public async Task<PspTokenResponse> RequestTokenAsync(PspTokenRequest request, CancellationToken cancellationToken)
    {
        var settings = Settings;

        try
        {
            var payload = new
            {
                action = "token",
                redirectUrl = settings.TopUpCallBackUrl,
                terminalId = settings.TerminalId,
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
                IpgUrl = $"{settings.BaseUrl}/OnlinePG/SendToken?token={samanResponse.Token}",
                RawStatus = samanResponse.Status.ToString(),
                RawErrorCode = samanResponse.ErrorCode,
                RawDescription = samanResponse.ErrorDesc
            };
        }
        catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _loggerService.Error(ex, $"{Code} token request timed out after {settings.TimeoutInSeconds} seconds");

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

    public Task<PspVerifyResponse> VerifyPaymentAsync(PspVerifyRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private static PspFailureReason MapFailureReason(string samanStatus)
    {
        return samanStatus switch
        {
            "1" => PspFailureReason.None,
            "-1" => PspFailureReason.InvalidTerminal,
            "-3" => PspFailureReason.InvalidAmount,
            "-4" => PspFailureReason.DuplicateReference,
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

    private class SamanTokenResponse
    {
        public int Status { get; set; }
        public string Token { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorDesc { get; set; } = string.Empty;
    }
}
