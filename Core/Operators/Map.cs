namespace Rx.NET.Core.Operators;

public static class MapExtension
{
    public static Observable<TOut> Map<TIn, TOut>(this Observable<TIn> obs, Func<TIn, TOut> mappingFunc)
    {
        return new Observable<TOut>(subscriber => { obs.Subscribe(new Observer<TIn>(x => subscriber.Next(mappingFunc(x)), subscriber.Complete)); });
    }
}