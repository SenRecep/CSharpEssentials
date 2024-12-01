namespace CSharpEssentials;

public readonly struct Any<T0, T1, T2, T3>
{
    private readonly object? _value;
    private readonly int _index;

    private Any(int index, object? value)
    {
        _index = index;
        _value = value;
    }

    public readonly int Index => _index;

    public bool IsFirst => _index == 0;
    public bool IsSecond => _index == 1;
    public bool IsThird => _index == 2;
    public bool IsFourth => _index == 3;

    public T0 AsFirst => _index == 0 ? (T0)_value! : throw InvalidOperation;
    public T1 AsSecond => _index == 1 ? (T1)_value! : throw InvalidOperation;
    public T2 AsThird => _index == 2 ? (T2)_value! : throw InvalidOperation;
    public T3 AsFourth => _index == 3 ? (T3)_value! : throw InvalidOperation;

    public static implicit operator Any<T0, T1, T2, T3>(T0 value) => new(0, value);
    public static implicit operator Any<T0, T1, T2, T3>(T1 value) => new(1, value);
    public static implicit operator Any<T0, T1, T2, T3>(T2 value) => new(2, value);
    public static implicit operator Any<T0, T1, T2, T3>(T3 value) => new(3, value);

    public void Switch(Action<T0> f0, Action<T1> f1, Action<T2> f2, Action<T3> f3)
    {
        switch (_index)
        {
            case 0: f0((T0)_value!); break;
            case 1: f1((T1)_value!); break;
            case 2: f2((T2)_value!); break;
            case 3: f3((T3)_value!); break;
            default: throw InvalidOperation;
        }
    }
    

    public TResult Match<TResult>(Func<T0, TResult> f0, Func<T1, TResult> f1, Func<T2, TResult> f2, Func<T3, TResult> f3)
    {
        return _index switch
        {
            0 => f0((T0)_value!),
            1 => f1((T1)_value!),
            2 => f2((T2)_value!),
            3 => f3((T3)_value!),
            _ => throw InvalidOperation
        };
    }

    public static Any<T0, T1, T2,T3> First(T0 value) => value;
    public static Any<T0, T1, T2, T3> Second(T1 value) => value;
    public static Any<T0, T1, T2, T3> Third(T2 value) => value;
    public static Any<T0, T1, T2, T3> Fourth(T3 value) => value;

    private static Exception InvalidOperation => new InvalidOperationException("No value");
}

