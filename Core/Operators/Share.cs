namespace Rx.NET.Core.Operators;

public static class ShareExtension
{
    public static Observable<TIn> Share<TIn>(this Observable<TIn> obs)
    {
        var subscribers = new List<IObserver<TIn>>();
        Subscription<TIn>? subscription = null;
        return new Observable<TIn>(subscriber =>
        {
            subscribers.Add(subscriber);

            subscription ??= obs.Subscribe(new Observer<TIn>(x => subscribers.ForEach(s => s.Next(x))));
        });
    }
}