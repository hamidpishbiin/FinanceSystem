using FinanceSystem.Domain.Payments;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FinanceSystem.Persistance.Mappings;

public class MoneyConverter() : ValueConverter<Money, decimal>(
    money => money.Value,
    value => new Money(value));
