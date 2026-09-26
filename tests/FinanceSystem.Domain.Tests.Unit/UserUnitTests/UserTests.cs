using FinanceSystem.Domain.Users.Exceptions;
using FluentAssertions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Tests.Unit.UserUnitTests;

public class UserTests
{
    private readonly UserBuilder _builder = new();

    [Fact]
    public async Task Create_should_properly_create_user()
    {
        var user = await _builder.Build();

        user.Id.Should().Be(UserBuilder.DefaultId);
        user.FirstName.Should().Be(UserBuilder.DefaultFirstName);
        user.LastName.Should().Be(UserBuilder.DefaultLastName);
        user.PhoneNumber.Should().Be(UserBuilder.DefaultPhoneNumber);
        user.NationalCode.Should().Be(UserBuilder.DefaultNationalCode);
    }

    [Fact]
    public async Task Create_should_throw_when_id_is_empty_guid()
    {
        Func<Task> act = () => _builder.WithId(Guid.Empty).Build();

        await act.Should().ThrowAsync<InvalidIdException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_firstName_is_null_or_empty_or_whiteSpace(string? firstName)
    {
        Func<Task> act = () => _builder.WithFirstName(firstName).Build();

        await act.Should().ThrowAsync<InvalidFirstNameException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_lastName_is_null_or_empty_or_whiteSpace(string? lastName)
    {
        Func<Task> act = () => _builder.WithLastName(lastName).Build();

        await act.Should().ThrowAsync<InvalidLastNameException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Create_should_throw_when_phoneNumber_is_null_or_empty_or_whiteSpace(string? phoneNumber)
    {
        Func<Task> act = () => _builder.WithPhoneNumber(phoneNumber).Build();

        await act.Should().ThrowAsync<InvalidPhoneNumberException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("          ")]
    [InlineData("001234567")]
    [InlineData("00123456789")]
    [InlineData("00123a5678")]
    [InlineData("００１２３４５６７８")]
    public async Task Create_should_throw_when_nationalCode_is_not_ten_ascii_digits(string? nationalCode)
    {
        Func<Task> act = () => _builder.WithNationalCode(nationalCode).Build();

        await act.Should().ThrowAsync<InvalidNationalCodeException>();
    }
}
