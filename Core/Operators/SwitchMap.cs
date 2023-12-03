namespace Rx.NET.Core.Operators;

public static class SwitchMapExtension
{
    private class Place<T>
    {
        public Subscription<T>? CurrentSubscription { get; set; }
        public bool Completed { get; set; }
    }

    public static Observable<TOut> SwitchMap<TIn, TOut>(this Observable<TIn> obs, Func<TIn, Observable<TOut>> spreadingFunc)
    {
        return new Observable<TOut>(subscriber =>
        {
            var place = new Place<TOut>();
            obs.Subscribe(new Observer<TIn>(x =>
            {
                lock (place)
                {
                    if (place.CurrentSubscription != null)
                    {
                        place.CurrentSubscription.Unsubscribe();
                    }

                    place.CurrentSubscription = spreadingFunc(x).Subscribe(new Observer<TOut>(y => subscriber.Next(y), () =>
                    {
                        lock (place)
                        {
                            place.CurrentSubscription = null;
                        }

                        if (place.Completed)
                            subscriber.Complete();
                    }));
                }
            }, () => place.Completed = true));
        });
    }
}