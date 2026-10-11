using System.Net.Http.Json;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.Payments.Enums;
using Microsoft.Extensions.Options;
using Shared.Core;

namespace FinanceSystem.Saman;

internal class SamanGateway : IPspGateway
{
    private const int SamanCallbackOkStatus = 2;

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

    public Task<PaymentVerificationResponse> VerifyPaymentAsync(PaymentVerificationRequest request, CancellationToken cancellationToken)
    {
        return SendTransactionAsync("/verifyTxnRandomSessionKey/ipg/VerifyTransaction", PspTransactionType.Verify, request, cancellationToken);
    }

    public Task<PaymentVerificationResponse> ReversePaymentAsync(PaymentVerificationRequest request, CancellationToken cancellationToken)
    {
        return SendTransactionAsync("/verifyTxnRandomSessionKey/ipg/ReverseTransaction", PspTransactionType.Reverse, request, cancellationToken);
    }

    private async Task<PaymentVerificationResponse> SendTransactionAsync(
        string path,
        PspTransactionType transactionType,
        PaymentVerificationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var payload = new
            {
                refNum = request.ReferenceNumber,
                terminalNumber = _settings.TerminalId
            };

            var httpClient = _httpClientFactory.CreateClient(nameof(PspCode.Saman));

            var response = await httpClient.PostAsJsonAsync(path, payload, cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadFromJsonAsync<SamanPaymentVerificationResponse>(cancellationToken);

            return new PaymentVerificationResponse()
            {
                HasResponse = responseBody is not null,
                IsSuccess = responseBody?.Success ?? false,
                Amount = responseBody?.TransactionDetail?.OrginalAmount,
                PspTransactionType = transactionType,
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
            _loggerService.Error(ex, $"{Code} {transactionType} request timed out after {_settings.TimeoutInSeconds} seconds");

            return MakeNoResponse(transactionType);
        }
        catch (HttpRequestException ex)
        {
            _loggerService.Error(ex, $"{Code} {transactionType} request failed.");

            return MakeNoResponse(transactionType);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex, $"{Code} {transactionType} request failed unexpectedly");

            return MakeNoResponse(transactionType);
        }
    }

    public PspCallback ParseCallback(IReadOnlyDictionary<string, string> fields)
    {
        var status = ParseInt(fields, "Status");
        var isSuccess = status == SamanCallbackOkStatus;

        return new PspCallback()
        {
            ReservationNumber = long.TryParse(Get(fields, "ResNum"), out var reservationNumber) ? reservationNumber : 0,
            IsSuccess = isSuccess,
            FailureReason = isSuccess ? null : MapCallbackFailureReason(status),
            RawStatus = Get(fields, "State") ?? string.Empty,
            RawStatusCode = status,
            Token = Get(fields, "Token"),
            ReferenceNumber = Get(fields, "RefNum"),
            Amount = decimal.TryParse(Get(fields, "Amount"), out var amount) ? amount : null,
            MaskedPan = Get(fields, "SecurePan"),
            RRN = Get(fields, "RRN"),
            TraceNumber = Get(fields, "TraceNo")
        };
    }

    private static PspFailureReason MapCallbackFailureReason(int? samanCallbackStatus)
    {
        return samanCallbackStatus switch
        {
            1 => PspFailureReason.CanceledByUser,
            3 => PspFailureReason.Rejected,
            4 => PspFailureReason.SessionExpired,
            5 or 10 or 11 => PspFailureReason.InvalidRequest,
            8 or 21 => PspFailureReason.MerchantConfigurationError,
            12 => PspFailureReason.InvalidTerminal,
            _ => PspFailureReason.Unknown
        };
    }

    private static string? Get(IReadOnlyDictionary<string, string> fields, string key)
    {
        return fields.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value) ? value : null;
    }

    private static int? ParseInt(IReadOnlyDictionary<string, string> fields, string key)
    {
        return int.TryParse(Get(fields, key), out var value) ? value : null;
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

    private static PaymentVerificationResponse MakeNoResponse(PspTransactionType transactionType)
    {
        return new PaymentVerificationResponse()
        {
            HasResponse = false,
            IsSuccess = false,
            PspTransactionType = transactionType
        };
    }
}
