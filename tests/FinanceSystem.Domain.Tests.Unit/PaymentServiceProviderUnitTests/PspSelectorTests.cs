using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FinanceSystem.Domain.PaymentServiceProviders.Services;
using FluentAssertions;
using NSubstitute;

namespace FinanceSystem.Domain.Tests.Unit.PaymentServiceProviderUnitTests;

public class PspSelectorTests
{
    private readonly IPspRepository _repository = Substitute.For<IPspRepository>();
    private readonly PspSelector _selector;

    public PspSelectorTests()
    {
        _selector = new PspSelector(_repository);
    }

    [Fact]
    public async Task SelectAsync_should_throw_when_requested_psp_is_inactive()
    {
        var inactivePsp = new PaymentServiceProviderBuilder().WithIsActive(false).Build();
        _repository.GetByCodeAsync(PspCode.Saman, Arg.Any<CancellationToken>()).Returns(inactivePsp);

        Func<Task> act = () => _selector.SelectAsync(PspCode.Saman, CancellationToken.None);

        await act.Should().ThrowAsync<PspNotAvailableException>();
    }

    [Fact]
    public async Task SelectAsync_should_return_highest_priority_active_psp_when_no_code_is_requested()
    {
        var psp = new PaymentServiceProviderBuilder().WithCode(PspCode.BehPardakht).Build();
        _repository.GetHighPriorityActiveAsync(Arg.Any<CancellationToken>()).Returns(psp);

        var selected = await _selector.SelectAsync(null, CancellationToken.None);

        selected.Should().BeSameAs(psp);
        await _repository.DidNotReceive().GetByCodeAsync(Arg.Any<PspCode>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SelectAsync_should_throw_when_no_active_psp_exists()
    {
        _repository.GetHighPriorityActiveAsync(Arg.Any<CancellationToken>()).Returns((PaymentServiceProvider?)null);

        Func<Task> act = () => _selector.SelectAsync(null, CancellationToken.None);

        await act.Should().ThrowAsync<PspNotAvailableException>();
    }
}
