using FinanceSystem.Domain.Users.Exceptions;

namespace FinanceSystem.Domain.Users;

public class User : EntityBase<long>
{
    private const int _nationalCodeLength = 10;

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string NationalCode { get; private set; } = string.Empty;

    private User()
    {
    }

    public User(
        string firstName,
        string lastName,
        string phoneNumber,
        string nationalCode)
    {
        Guard<InvalidFirstNameException>.AgainstNullOrEmpty(firstName);
        Guard<InvalidLastNameException>.AgainstNullOrEmpty(lastName);
        Guard<InvalidPhoneNumberException>.AgainstNullOrEmpty(phoneNumber);
        Guard<InvalidNationalCodeException>.IsFalse(IsValidNationalCode(nationalCode));

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        NationalCode = nationalCode;
    }

    private static bool IsValidNationalCode(string? nationalCode)
    {
        return nationalCode is { Length: _nationalCodeLength } && nationalCode.All(char.IsAsciiDigit);
    }
}
