namespace Rx.NET.Core.Operators;

public static class DelayExtension
{
    public static Observable<TIn> Delay<TIn>(this Observable<TIn> obs, int milliseconds)
    {
        return new Observable<TIn>(subscriber =>
        {
            obs.Subscribe(x => Task.Delay(milliseconds).ContinueWith(_ => subscriber.Next(x)));
        });
    }
}