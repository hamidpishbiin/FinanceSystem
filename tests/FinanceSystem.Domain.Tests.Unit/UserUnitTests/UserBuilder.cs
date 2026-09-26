using FinanceSystem.Domain.Users;

namespace FinanceSystem.Domain.Tests.Unit.UserUnitTests;

public class UserBuilder
{
    public const string DefaultIdString = "7b2e4f10-8c3d-4a9b-b1e6-5f0a2c7d9e34";
    public const string DefaultFirstName = "Ali";
    public const string DefaultLastName = "Rezaei";
    public const string DefaultPhoneNumber = "09121234567";
    public const string DefaultNationalCode = "0012345678";

    public static readonly Guid DefaultId = Guid.Parse(DefaultIdString);

    private Guid Id { get; set; } = DefaultId;
    private string FirstName { get; set; } = DefaultFirstName;
    private string LastName { get; set; } = DefaultLastName;
    private string PhoneNumber { get; set; } = DefaultPhoneNumber;
    private string NationalCode { get; set; } = DefaultNationalCode;

    public async Task<User> Build()
    {
        return await User.Create(
            Id,
            FirstName,
            LastName,
            PhoneNumber,
            NationalCode);
    }

    public UserBuilder WithId(Guid id)
    {
        Id = id;
        return this;
    }

    public UserBuilder WithFirstName(string? firstName)
    {
        FirstName = firstName!;
        return this;
    }

    public UserBuilder WithLastName(string? lastName)
    {
        LastName = lastName!;
        return this;
    }

    public UserBuilder WithPhoneNumber(string? phoneNumber)
    {
        PhoneNumber = phoneNumber!;
        return this;
    }

    public UserBuilder WithNationalCode(string? nationalCode)
    {
        NationalCode = nationalCode!;
        return this;
    }
}
