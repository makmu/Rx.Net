using System.Collections;

namespace Rx.NET.Core;

public interface IObserver<in T>
{
    public Action<T> Next { get; }
    public Action<string> Error { get; }
    public Action Complete { get; }
}

public class Observable<T>
{
    private readonly Action<IObserver<T>> _generatorFunction;

    public Observable(Action<IObserver<T>> generatorFunction)
    {
        _generatorFunction = generatorFunction;
    }

    public Subscription<T> Subscribe(IObserver<T> observer)
    {
        var subscription = new Subscription<T>(observer);
        _generatorFunction(subscription);
        return subscription;
    }

    public Subscription<T> Subscribe(Action<T> next)
    {
        return Subscribe(new Observer<T>(next));
    }
}