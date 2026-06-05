using System.Text.RegularExpressions;
using Sklep.Domain.Common;

namespace Sklep.Domain.ValueObjects;

public sealed record EmailAddress
{
    private static readonly Regex EmailRegex = new(
        "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public string Value { get; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Email is required.");
        }

        var normalized = value.Trim().ToLowerInvariant();
        if (!EmailRegex.IsMatch(normalized))
        {
            throw new DomainException("Email has invalid format.");
        }

        Value = normalized;
    }

    public override string ToString()
    {
        return Value;
    }
}
