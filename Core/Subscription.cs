namespace Rx.NET.Core;

public class Subscription<T> : IObserver<T>
{
    private IObserver<T>? _observer;

    public Subscription(IObserver<T> observer)
    {
        _observer = observer;
    }

    public void Unsubscribe()
    {
        _observer = null;
    }

    public Action<T> Next => x =>
    {
        if (_observer != null) _observer.Next(x);
    };

    public Action<string> Error => err =>
    {
        if (_observer != null) _observer.Error(err);
    };

    public Action Complete => () =>
    {
        if (_observer != null) _observer.Complete();
    };
}