namespace Sklep.Application.Common;

public sealed class BusinessRuleViolationException : Exception
{
    public BusinessRuleViolationException(string message) : base(message)
    {
    }
}
