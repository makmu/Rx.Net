// See https://aka.ms/new-console-template for more information

using Rx.NET.Core;
using Rx.NET.Core.Operators;

var observable = new Observable<string>((subscriber) =>
{
    subscriber.Next("1");
    subscriber.Next("2");
    subscriber.Next("3");
    Task.Delay(2000).ContinueWith(_ =>
    {
        subscriber.Next("4");
        subscriber.Complete();
    });
});

var worker = new AsyncWorker();

/*observable
    .Tap(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: tapped {x}"))
    .MergeMap(x => worker.Do(x))
    .Subscribe(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: {x}"));*/

/*observable
    .Tap(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: tapped {x}"))
    .ConcatMap(x => worker.Do(x))
    .Subscribe(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: {x}"));*/

/*observable
    .Tap(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: tapped {x}"))
    .SwitchMap(x => worker.Do(x))
    .Subscribe(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: {x}"));*/

/*observable
    .Tap(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: tapped {x}"))
    .ExhaustMap(x => worker.Do(x))
    .Subscribe(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: {x}"));*/

/*var obs = observable
    .Tap(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: tapped {x}"))
    .Map(_ => DateTimeOffset.Now)
    .Share();
    
obs.Subscribe(x => Console.WriteLine($"sub 1: {DateTimeOffset.Now.ToUnixTimeMilliseconds()}: {x.ToUnixTimeMilliseconds()}"));
obs.Subscribe(x => Console.WriteLine($"sub 2: {DateTimeOffset.Now.ToUnixTimeMilliseconds()}: {x.ToUnixTimeMilliseconds()}"));*/

/*observable
    .Tap(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: tapped {x}"))
    .Delay(1200)
    .Subscribe(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: {x}"));*/

observable
    .Tap(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: tapped {x}"))
    .DebounceTime(300)
    .Subscribe(x => Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}: {x}"));

Thread.Sleep(10000);

public class AsyncWorker 
{
    public Observable<string> Do(string id)
    {
        return new Observable<string>(s =>
        {
            s.Next($"starting on {id}");
            Task.Delay(int.Parse(id) * 1000).ContinueWith(_ =>
            {
                s.Next($"finished with {id}");
                s.Complete();
            });
        });
    }

    public Observable<int> Interval(int i)
    {
        return new Observable<int>(s =>
        {
            NextInterval(i, s, new Place(0));
        });
    }

    private void NextInterval(int i, Rx.NET.Core.IObserver<int> s, Place p)
    {
        Task.Delay(i).ContinueWith(_ =>
        {
            s.Next(p.N++);
            NextInterval(i, s, p);
        });
    }

    private class Place
    {
        public int N { get; set; }

        public Place(int n)
        {
            N = n;
        }
    }
}