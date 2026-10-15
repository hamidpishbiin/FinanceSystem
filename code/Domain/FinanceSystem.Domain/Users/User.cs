using FinanceSystem.Domain.Users.Exceptions;

namespace FinanceSystem.Domain.Users;

public class User : EntityBase<long>
{
    private const int NationalCodeLength = 10;

    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public string NationalCode { get; private set; } = default!;

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

    private static bool IsValidNationalCode(string? nationalCode) =>
        nationalCode is { Length: NationalCodeLength } && nationalCode.All(char.IsAsciiDigit);
}
