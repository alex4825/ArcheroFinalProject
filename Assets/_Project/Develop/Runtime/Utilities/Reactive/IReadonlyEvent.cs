using System;

namespace Assets._Project.Develop.Runtime.Utilities.Reactive
{
    public interface IReadonlyEvent
    {
        IDisposable Subscribe(Action action);
    }

    public interface IReadonlyEvent<T>
    {
        IDisposable Subscribe(Action<T> action);
    }

    public interface IReadonlyEvent<T1, T2>
    {
        IDisposable Subscribe(Action<T1, T2> action);
    }
}
