using Sklep.Domain.Common;

namespace Sklep.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "PLN")
    {
        if (amount < 0)
        {
            throw new DomainException("Money amount cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new DomainException("Currency is required.");
        }

        Amount = decimal.Round(amount, 2);
        Currency = currency.Trim().ToUpperInvariant();
    }

    public static Money Zero(string currency = "PLN")
    {
        return new Money(0, currency);
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(int quantity)
    {
        if (quantity < 0)
        {
            throw new DomainException("Quantity cannot be negative.");
        }

        return new Money(Amount * quantity, Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (!string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase))
        {
            throw new DomainException("Cannot calculate money values in different currencies.");
        }
    }
}
