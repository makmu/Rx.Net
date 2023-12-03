namespace Rx.NET.Core.Operators;

public static class TakeUntilExtension
{
    public static Observable<TIn> TakeUntil<TIn, TNotifier>(this Observable<TIn> obs, Observable<TNotifier> notifier)
    {
        return new Observable<TIn>(subscriber =>
        {
            notifier.Subscribe(new Observer<TNotifier>(_ => subscriber.Complete()));
            obs.Subscribe(subscriber);
        });
    }
}