namespace Rx.NET.Core.Operators;

public static class DebounceTimeExtension
{
    public static Observable<TIn> DebounceTime<TIn>(this Observable<TIn> obs, int milliseconds)
    {
        var tokenSource = new CancellationTokenSource();
        return new Observable<TIn>(subscriber =>
        {
            obs.Subscribe(x =>
            {
                tokenSource.Cancel();
                Task.Delay(milliseconds, tokenSource.Token).ContinueWith(_ => subscriber.Next(x), tokenSource.Token);
            });
        });
    }
}