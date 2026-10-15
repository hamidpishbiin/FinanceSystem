using FinanceSystem.Domain.Users;

namespace FinanceSystem.Domain.Tests.Unit.UserUnitTests;

public class UserBuilder
{
    public const string DefaultFirstName = "Ali";
    public const string DefaultLastName = "Rezaei";
    public const string DefaultPhoneNumber = "09121234567";
    public const string DefaultNationalCode = "0012345678";

    private string FirstName { get; set; } = DefaultFirstName;
    private string LastName { get; set; } = DefaultLastName;
    private string PhoneNumber { get; set; } = DefaultPhoneNumber;
    private string NationalCode { get; set; } = DefaultNationalCode;

    public User Build()
    {
        return new User(
            FirstName,
            LastName,
            PhoneNumber,
            NationalCode);
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
