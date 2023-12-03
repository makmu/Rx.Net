using System.Collections.Concurrent;

namespace Rx.NET.Core.Operators;

public static class ConcatMapExtension
{
    private class Place<T>
    {
        public Subscription<T>? CurrentSubscription { get; set; }
        public bool Completed { get; set; }
    }

    private static void SubscribeNext<TIn, TOut>(ConcurrentQueue<TIn> queue, Func<TIn, Observable<TOut>> spreadingFunc, IObserver<TOut> subscriber, Place<TOut> place)
    {
        lock (place)
        {
            if (place.CurrentSubscription == null && queue.TryDequeue(out var nextValue))
            {
                place.CurrentSubscription = spreadingFunc(nextValue).Subscribe(new Observer<TOut>(x => subscriber.Next(x), () =>
                {
                    lock (place)
                    {
                        place.CurrentSubscription = null;
                    }

                    SubscribeNext(queue, spreadingFunc, subscriber, place);
                }));
            }
            else if (place.Completed)
            {
                subscriber.Complete();
            }
        }
    }

    public static Observable<TOut> ConcatMap<TIn, TOut>(this Observable<TIn> obs, Func<TIn, Observable<TOut>> spreadingFunc)
    {
        return new Observable<TOut>(subscriber =>
        {
            var place = new Place<TOut>();
            var queue = new ConcurrentQueue<TIn>();
            obs.Subscribe(new Observer<TIn>(x =>
            {
                queue.Enqueue(x);
                SubscribeNext(queue, spreadingFunc, subscriber, place);
            }, () => place.Completed = true));
        });
    }
}