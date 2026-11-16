using System.Net.Http.Json;
using System.Text.Json;
using Autofac.Features.OwnedInstances;
using FinanceSystem.Application.Contracts.Gateways;
using FinanceSystem.Application.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Application.Contracts.Payments;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Exceptions;
using Microsoft.Extensions.Options;
using Shared.Core;

namespace FinanceSystem.Saman;

internal class SamanGateway(
    IOptions<PspOptions> pspOptions,
    IHttpClientFactory httpClientFactory,
    ILoggerService loggerService,
    ISystemClock systemClock,
    Func<Owned<SamanExchangeLogWriter>> exchangeLogWriterFactory) : IPspGateway
{
    private const int _samanCallbackOkStatus = 2;
    private const int _tokenResponseSuccessStatus = 1;

    private readonly PspSettings _settings = pspOptions.Value.Saman;

    public PspCode Code => PspCode.Saman;

    public async Task<PspTokenResponse> RequestPaymentTokenAsync(PspTokenRequest request, CancellationToken cancellationToken)
    {
        var exchangeLog = new SamanTokenExchangeLogModel
        {
            CreatedAt = systemClock.GetNow()
        };

        try
        {
            var payload = new
            {
                action = "token",
                redirectUrl = request.CallbackUrl,
                terminalId = _settings.TerminalId,
                amount = request.Amount,
                resNum = request.ReservationNumber
            };

            exchangeLog.RequestAmount = payload.amount;
            exchangeLog.RequestReservationNumber = payload.resNum;

            var httpClient = httpClientFactory.CreateClient(nameof(PspCode.Saman));

            using var message = new HttpRequestMessage(HttpMethod.Post, "/OnlinePG/OnlinePG");

            message.Content = JsonContent.Create(payload);

            var serializedResponse = await httpClient.SendAsync(message, cancellationToken);

            exchangeLog.HttpStatusCode = (int)serializedResponse.StatusCode;

            var rawResponseBody = await serializedResponse.Content.ReadAsStringAsync(cancellationToken);

            if (!serializedResponse.IsSuccessStatusCode)
            {
                exchangeLog.RawResponseBody = rawResponseBody;
                exchangeLog.Response = null;
                throw new HttpRequestException("Saman token request did not return with success status code.");
            }

            exchangeLog.RawResponseBody = rawResponseBody;

            var response = await serializedResponse.Content.ReadFromJsonAsync<SamanTokenResponse>(cancellationToken);

            if (response is null)
            {
                exchangeLog.Response = null;
                throw new InvalidOperationException("Saman token response was null.");
            }

            exchangeLog.Response = response;

            var isSuccess = response.Status == _tokenResponseSuccessStatus;

            return new PspTokenResponse
            {
                IsSuccess = isSuccess,
                Token = response.Token,
                Handoff = isSuccess
                    ? MakeHandoff(response.Token)
                    : null
            };
        }
        catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            exchangeLog.RecordError(ex);

            throw;
        }
        catch (OperationCanceledException ex)
        {
            exchangeLog.RecordError(ex);

            loggerService.Error(ex, $"{Code} token request timed out after {_settings.TimeoutInSeconds} seconds");

            return MakeFailedTokenResponse();
        }
        catch (HttpRequestException ex)
        {
            exchangeLog.RecordError(ex);

            loggerService.Error(ex, $"{Code} token request failed.");

            return MakeFailedTokenResponse();
        }
        catch (Exception ex)
        {
            exchangeLog.RecordError(ex);

            loggerService.Error(ex, $"{Code} token request failed unexpectedly");

            return MakeFailedTokenResponse();
        }
        finally
        {
            loggerService.Information($"TokenExchangeLog {exchangeLog}");
            await WriteExchangeLogAsync(exchangeLog);
        }
    }

    private async Task WriteExchangeLogAsync<TLog>(TLog exchangeLog) where TLog : class
    {
        try
        {
            using var exchangeLogWriter = exchangeLogWriterFactory();

            await exchangeLogWriter.Value.WriteAsync(exchangeLog, CancellationToken.None);
        }
        catch (Exception ex)
        {
            loggerService.Error(ex, "{PspGateway} {ExchangeLog} could not be saved", Code, typeof(TLog).Name);
        }
    }

    public async Task<PaymentVerificationResponse> VerifyPaymentAsync(PaymentVerificationRequest request, CancellationToken cancellationToken)
    {
        var result = new PaymentVerificationResponse()
        {
            TransactionType = PspTransactionType.Verification
        };

        var exchangeLog = new SamanVerifyExchangeLogModel
        {
            CreatedAt = systemClock.GetNow()
        };

        try
        {

            var payload = new
            {
                refNum = request.ReferenceNumber,
                terminalNumber = _settings.TerminalId
            };

            exchangeLog.RequestReferenceNumber = payload.refNum;

            var httpClient = httpClientFactory.CreateClient(nameof(PspCode.Saman));

            using var message = new HttpRequestMessage(HttpMethod.Post, "/verifyTxnRandomSessionKey/ipg/VerifyTransaction");

            message.Content = JsonContent.Create(payload);

            var response = await httpClient.SendAsync(message, cancellationToken);

            exchangeLog.HttpStatusCode = (int)response.StatusCode;

            exchangeLog.RawResponseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Saman verify request did not return with success status code.");
            }

            var responseBody = await response.Content.ReadFromJsonAsync<SamanPaymentVerificationResponse>(cancellationToken);

            exchangeLog.Response = responseBody;

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

            result.IsSuccess = responseBody.Success ?? false;
            result.Amount = responseBody.TransactionDetail?.OrginalAmount;
            result.Code = responseBody.ResultCode!.Value;

            return result;
        }
        catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            exchangeLog.RecordError(ex);

            throw;
        }
        catch (OperationCanceledException ex)
        {
            exchangeLog.RecordError(ex);

            loggerService.Error(ex, "{PspGateway} {TransactionType} request timed out after {Timeout} seconds", Code, result.TransactionType, _settings.TimeoutInSeconds);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
        catch (HttpRequestException ex)
        {
            exchangeLog.RecordError(ex);

            loggerService.Error(ex, "{PspGateway} {TransactionType} request failed.", Code, result.TransactionType);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
        catch (Exception ex)
        {
            exchangeLog.RecordError(ex);

            loggerService.Error(ex, "{PspGateway} {TransactionType} request failed unexpectedly", Code, result.TransactionType);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
        finally
        {
            await WriteExchangeLogAsync(exchangeLog);
        }
    }

    private async Task<PaymentVerificationResponse> ReversePaymentAsync(PaymentVerificationRequest request, CancellationToken cancellationToken)
    {
        var result = new PaymentVerificationResponse()
        {
            TransactionType = PspTransactionType.Reverse
        };

        var exchangeLog = new SamanReverseExchangeLogModel
        {
            CreatedAt = systemClock.GetNow()
        };

        try
        {
            var payload = new
            {
                refNum = request.ReferenceNumber,
                terminalNumber = _settings.TerminalId
            };

            exchangeLog.RequestReferenceNumber = payload.refNum;

            var httpClient = httpClientFactory.CreateClient(nameof(PspCode.Saman));

            using var message = new HttpRequestMessage(HttpMethod.Post, "/verifyTxnRandomSessionKey/ipg/ReverseTransaction");

            message.Content = JsonContent.Create(payload);

            var response = await httpClient.SendAsync(message, cancellationToken);

            exchangeLog.HttpStatusCode = (int)response.StatusCode;

            exchangeLog.RawResponseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Saman reverse request did not return with success status code.");
            }

            var responseBody = await response.Content.ReadFromJsonAsync<SamanPaymentVerificationResponse>(cancellationToken);

            exchangeLog.Response = responseBody;

            if (responseBody?.Success is not true)
            {
                throw new PaymentReverseFailedException(PaymentExceptionCodes.PaymentReverseFailed, $"{result.TransactionType} failed for ReferenceNUmber: {request.ReferenceNumber}");
            }

            result.IsSuccess = false;

            return result;
        }
        catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            exchangeLog.RecordError(ex);

            throw;
        }
        catch (OperationCanceledException ex)
        {
            exchangeLog.RecordError(ex);

            loggerService.Error(ex, "{TransactionType} request timed out after {Timeout} seconds", result.TransactionType, _settings.TimeoutInSeconds);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
        catch (HttpRequestException ex)
        {
            exchangeLog.RecordError(ex);

            loggerService.Error(ex, "{TransactionType} request failed.", result.TransactionType);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
        catch (Exception ex)
        {
            exchangeLog.RecordError(ex);

            loggerService.Error(ex, "{TransactionType} request failed unexpectedly", result.TransactionType);

            return MakeFailedVerificationResponse(result.TransactionType);
        }
        finally
        {
            await WriteExchangeLogAsync(exchangeLog);
        }
    }

    public async Task<PspCallback> ParseCallbackAsync(IReadOnlyDictionary<string, string> fields, CancellationToken cancellationToken)
    {
        var callbackLog = new SamanCallbackLogModel
        {
            CreatedAt = systemClock.GetNow(),
            RawFields = JsonSerializer.Serialize(fields)
        };

        try
        {
            var status = int.Parse(fields["Status"]);

            var isSuccess = status == _samanCallbackOkStatus;

            var callback = new PspCallback
            {
                ReservationNumber = long.Parse(fields["ResNum"]),
                IsSuccess = isSuccess,
                Token = fields["Token"],
                ReferenceNumber = fields["RefNum"],
                Amount = new Money(decimal.Parse(fields["Amount"])),
                MaskedPan = fields["SecurePan"],
                RRN = fields["RRN"],
                TraceNumber = fields["TraceNo"]
            };

            callbackLog.Status = status;
            callbackLog.ResNum = callback.ReservationNumber;
            callbackLog.Token = callback.Token;
            callbackLog.RefNum = callback.ReferenceNumber;
            callbackLog.Amount = callback.Amount.Value;
            callbackLog.SecurePan = callback.MaskedPan;
            callbackLog.RRN = callback.RRN;
            callbackLog.TraceNo = callback.TraceNumber;

            return callback;
        }
        catch (Exception ex)
        {
            callbackLog.RecordError(ex);

            throw;
        }
        finally
        {
            await WriteExchangeLogAsync(callbackLog);
        }
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

    private PspHandoff MakeHandoff(string token)
    {
        return new PspHandoff(
            $"{_settings.BaseUrl}/OnlinePG/SendToken?token={Uri.EscapeDataString(token)}",
            HttpMethod.Get.Method,
            new Dictionary<string, string>());
    }
}
