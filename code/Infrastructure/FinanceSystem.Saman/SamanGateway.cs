using System.Net.Http.Json;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using Microsoft.Extensions.Options;
using Shared.Core;

namespace FinanceSystem.Saman;

internal class SamanGateway : IPspGateway
{
    private const int _samanCallbackOkStatus = 2;

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

    public async Task<PspTokenResponse> RequestPaymentTokenAsync(PspTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = new
            {
                action = "token",
                redirectUrl = _settings.TopUpCallBackUrl,
                terminalId = _settings.TerminalId,
                amount = request.Amount,
                resNum = request.ReservationNumber
            };

            var httpClient = _httpClientFactory.CreateClient(nameof(PspCode.Saman));

            var apiResponse = await httpClient.PostAsJsonAsync("/OnlinePG/OnlinePG", payload, cancellationToken);

            apiResponse.EnsureSuccessStatusCode();

            var samanResponse = await apiResponse.Content.ReadFromJsonAsync<SamanTokenResponse>(cancellationToken);

            if (samanResponse is null)
            {
                throw new InvalidOperationException("Saman token response was null.");
            }

            var isSamanResponseSuccess = samanResponse.Status == 1;

            return new PspTokenResponse()
            {
                IsSuccess = isSamanResponseSuccess,
                Token = samanResponse.Token,
                IpgUrl = $"{_settings.BaseUrl}/OnlinePG/SendToken?token={samanResponse.Token}",
                Status = samanResponse.Status.ToString(),
                StatusCode = samanResponse.ErrorCode,
                Description = samanResponse.ErrorDesc
            };
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _loggerService.Error(ex, $"{Code} token request timed out after {_settings.TimeoutInSeconds} seconds");

            return MakeFailedTokenResponse();
        }
        catch (HttpRequestException ex)
        {
            _loggerService.Error(ex, $"{Code} token request failed.");

            return MakeFailedTokenResponse();
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex, $"{Code} token request failed unexpectedly");

            return MakeFailedTokenResponse();
        }
    }

    public async Task<PaymentVerificationResponse> VerifyPaymentAsync(PaymentVerificationRequest request, CancellationToken cancellationToken)
    {
        var result = new PaymentVerificationResponse()
        {
            TransactionType = PspTransactionType.Verification
        };

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

            if (responseBody?.Success is null || responseBody.TransactionDetail is null)
            {
                return await ReversePaymentAsync(request, cancellationToken);
            }

            if (responseBody.Success is false ||
                responseBody.TransactionDetail.AffectiveAmount != request.Amount ||
                responseBody.TransactionDetail.OrginalAmount != request.Amount)
            {
                result.IsSuccess = false;

                return result;
            }

            result.IsSuccess = responseBody?.Success ?? false;
            result.Amount = responseBody?.TransactionDetail?.OrginalAmount;
            result.Code = responseBody!.ResultCode!.Value;

            return result;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _loggerService.Error(ex, "{PspGateway} {TransactionType} request timed out after {Timeout} seconds", Code, result.TransactionType, _settings.TimeoutInSeconds);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
        catch (HttpRequestException ex)
        {
            _loggerService.Error(ex, "{PspGateway} {TransactionType} request failed.", Code, result.TransactionType);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex, "{PspGateway} {TransactionType} request failed unexpectedly", Code, result.TransactionType);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
    }

    private async Task<PaymentVerificationResponse> ReversePaymentAsync(PaymentVerificationRequest request, CancellationToken cancellationToken)
    {
        var result = new PaymentVerificationResponse()
        {
            TransactionType = PspTransactionType.Reverse
        };

        try
        {
            var payload = new
            {
                refNum = request.ReferenceNumber,
                terminalNumber = _settings.TerminalId
            };

            var httpClient = _httpClientFactory.CreateClient(nameof(PspCode.Saman));

            var response = await httpClient.PostAsJsonAsync("/verifyTxnRandomSessionKey/ipg/ReverseTransaction", payload, cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadFromJsonAsync<SamanPaymentVerificationResponse>(cancellationToken);

            if (responseBody?.Success is not true)
            {
                throw new PaymentReverseFailedException(PaymentExceptionCodes.PaymentReverseFailed, $"{result.TransactionType} failed for ReferenceNUmber: {request.ReferenceNumber}");
            }

            result.IsSuccess = false;

            return result;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _loggerService.Error(ex, "{TransactionType} request timed out after {Timeout} seconds", result.TransactionType, _settings.TimeoutInSeconds);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
        catch (HttpRequestException ex)
        {
            _loggerService.Error(ex, "{TransactionType} request failed.", result.TransactionType);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex, "{TransactionType} request failed unexpectedly", result.TransactionType);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
    }

    public PspCallback ParseCallback(IReadOnlyDictionary<string, string> fields)
    {
        var status = int.Parse(fields["Status"]);

        var isSuccess = status == _samanCallbackOkStatus;

        return new PspCallback
        {
            ReservationNumber = long.Parse(fields["ResNum"]),
            IsSuccess = isSuccess,
            Status = fields["State"],
            StatusCode = status,
            Token = fields["Token"],
            ReferenceNumber = fields["RefNum"],
            Amount = decimal.Parse(fields["Amount"]),
            MaskedPan = fields["SecurePan"],
            RRN = fields["RRN"],
            TraceNumber = fields["TraceNo"]
        };
    }

    private PspTokenResponse MakeFailedTokenResponse()
    {
        return new PspTokenResponse()
        {
            IsSuccess = false,
        };
    }

    private static PaymentVerificationResponse MakeFailedVerificationResponse(PspTransactionType transactionType)
    {
        return new PaymentVerificationResponse()
        {
            IsSuccess = false,
            TransactionType = transactionType
        };
    }
}
