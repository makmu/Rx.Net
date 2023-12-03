namespace Rx.NET.Core.Operators;

public static class TabExtension
{
    public static Observable<T> Tap<T>(this Observable<T> obs, Action<T> tapFunc)
    {
        return new Observable<T>(subscriber =>
        {
            obs.Subscribe(new Observer<T>(x =>
            {
                tapFunc(x);
                subscriber.Next(x);
            }, subscriber.Complete));
        });
    }
}