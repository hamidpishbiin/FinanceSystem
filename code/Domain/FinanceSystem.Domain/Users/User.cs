using FinanceSystem.Domain.Users.Exceptions;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.Users;

public class User : EntityBase<Guid>
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
        Guid id,
        string firstName,
        string lastName,
        string phoneNumber,
        string nationalCode)
    {
        Guard<InvalidIdException>.IsTrue(id == Guid.Empty);
        Guard<InvalidFirstNameException>.AgainstNullOrEmpty(firstName);
        Guard<InvalidLastNameException>.AgainstNullOrEmpty(lastName);
        Guard<InvalidPhoneNumberException>.AgainstNullOrEmpty(phoneNumber);
        Guard<InvalidNationalCodeException>.IsFalse(IsValidNationalCode(nationalCode));

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        NationalCode = nationalCode;
    }

    private static bool IsValidNationalCode(string? nationalCode) =>
        nationalCode is { Length: NationalCodeLength } && nationalCode.All(char.IsAsciiDigit);
}
