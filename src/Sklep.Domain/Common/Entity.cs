namespace Sklep.Domain.Common;

public abstract class Entity
{
    public int Id { get; protected set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Entity other || GetType() != other.GetType())
        {
            return false;
        }

        return Id != 0 && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id == 0 ? base.GetHashCode() : HashCode.Combine(GetType(), Id);
    }
}
