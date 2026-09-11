using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.PaymentServiceProviderUnitTests;

public class PaymentServiceProviderTests
{
    private readonly PaymentServiceProviderBuilder _builder = new();

    [Fact]
    public async Task Create_should_properly_create_paymentServiceProvider()
    {
        var psp = await _builder.Build();

        psp.Id.Should().Be(PaymentServiceProviderBuilder.DefaultId);
        psp.Code.Should().Be(PaymentServiceProviderBuilder.DefaultCode);
        psp.Name.Should().Be(PaymentServiceProviderBuilder.DefaultName);
        psp.IsActive.Should().Be(PaymentServiceProviderBuilder.DefaultIsActive);
        psp.Priority.Should().Be(PaymentServiceProviderBuilder.DefaultPriority);
        psp.MerchantId.Should().Be(PaymentServiceProviderBuilder.DefaultMerchantId);
        psp.TerminalId.Should().Be(PaymentServiceProviderBuilder.DefaultTerminalId);
        psp.CredentialsRef.Should().Be(PaymentServiceProviderBuilder.DefaultCredentialsRef);
        psp.BaseUrl.Should().Be(PaymentServiceProviderBuilder.DefaultBaseUrl);
        psp.CallbackUrl.Should().Be(PaymentServiceProviderBuilder.DefaultCallbackUrl);
    }

    [Fact]
    public async Task Create_should_throw_when_id_is_empty_guid()
    {
        Func<Task> act = () => _builder.WithId(Guid.Empty).Build();

        await act.Should().ThrowAsync<InvalidIdException>();
    }

    [Theory]
    [InlineData((PsPCode)0)]
    [InlineData((PsPCode)99)]
    public async Task Create_should_throw_when_code_is_not_defined(PsPCode code)
    {
        Func<Task> act = () => _builder.WithCode(code).Build();

        await act.Should().ThrowAsync<InvalidPspCodeException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_name_is_null_or_empty_or_whiteSpace(string? name)
    {
        Func<Task> act = () => _builder.WithName(name).Build();

        await act.Should().ThrowAsync<InvalidPspNameException>();
    }

    [Theory]
    [InlineData((short)-1)]
    [InlineData(short.MinValue)]
    public async Task Create_should_throw_when_priority_is_negative(short priority)
    {
        Func<Task> act = () => _builder.WithPriority(priority).Build();

        await act.Should().ThrowAsync<InvalidPriorityException>();
    }

    [Fact]
    public async Task Create_should_allow_zero_priority()
    {
        var psp = await _builder.WithPriority(0).Build();

        psp.Priority.Should().Be(0);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_merchantId_is_null_or_empty_or_whiteSpace(string? merchantId)
    {
        Func<Task> act = () => _builder.WithMerchantId(merchantId).Build();

        await act.Should().ThrowAsync<InvalidMerchantIdException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_credentialsRef_is_null_or_empty_or_whiteSpace(string? credentialsRef)
    {
        Func<Task> act = () => _builder.WithCredentialsRef(credentialsRef).Build();

        await act.Should().ThrowAsync<InvalidCredentialsRefException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_baseUrl_is_null_or_empty_or_whiteSpace(string? baseUrl)
    {
        Func<Task> act = () => _builder.WithBaseUrl(baseUrl).Build();

        await act.Should().ThrowAsync<InvalidBaseUrlException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_callbackUrl_is_null_or_empty_or_whiteSpace(string? callbackUrl)
    {
        Func<Task> act = () => _builder.WithCallbackUrl(callbackUrl).Build();

        await act.Should().ThrowAsync<InvalidCallbackUrlException>();
    }

    [Fact]
    public async Task Create_should_allow_null_terminalId()
    {
        var psp = await _builder.WithTerminalId(null).Build();

        psp.TerminalId.Should().BeNull();
    }

    [Fact]
    public async Task Create_should_allow_inactive_provider()
    {
        var psp = await _builder.WithIsActive(false).Build();

        psp.IsActive.Should().BeFalse();
    }

    [Fact]
    public void PsPCode_values_are_persisted_contract_and_should_not_change()
    {
        ((int)PsPCode.Saman).Should().Be(1);
        ((int)PsPCode.BehPardakht).Should().Be(2);

        Enum.GetValues<PsPCode>().Should().HaveCount(2,
            "adding a provider requires a matching row in PaymentServiceProviders and any Code check constraint");
    }
}
