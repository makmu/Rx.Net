namespace Rx.NET.Core;

public class Observer<T> : IObserver<T>
{
    public Observer(Action<T> next) : this(next, Console.WriteLine, () => Console.WriteLine("Done"))
    {
    }

    public Observer(Action<T> next, Action complete) : this(next, Console.WriteLine, complete)
    {
    }

    public Observer(Action<T> next, Action<string> error, Action complete)
    {
        Next = x =>
        {
            if(!IsCompleted) next(x);
        };
        Error = (err) =>
        {
            if(!IsCompleted) error(err);
        };
        Complete = () =>
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                complete();
            }
        };
    }

    private bool IsCompleted { get; set; }
    public Action<T> Next { get; }
    public Action<string> Error { get; }
    public Action Complete { get; }
}