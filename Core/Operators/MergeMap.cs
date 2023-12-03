namespace Rx.NET.Core.Operators;

public static class MergeMapExtension
{
    public static Observable<TOut> MergeMap<TIn, TOut>(this Observable<TIn> obs, Func<TIn, Observable<TOut>> spreadingFunc)
    {
        return new Observable<TOut>(subscriber => { obs.Subscribe(x => spreadingFunc(x).Subscribe(y => subscriber.Next(y))); });
    }
}