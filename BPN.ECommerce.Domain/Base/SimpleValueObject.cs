namespace BPN.ECommerce.Domain.Base;

[Serializable]
public abstract class SimpleValueObject<T>(T value)
    where T : IComparable
{
    protected SimpleValueObject() : this(default)
    {
    }

    public T Value { get; } = value;


    public override string ToString()
    {
        return Value?.ToString();
    }

    public static implicit operator T(SimpleValueObject<T> valueObject)
    {
        return valueObject.Value;
    }
}