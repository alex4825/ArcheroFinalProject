using System;

namespace Assets._Project.Develop.Runtime.Utilities.Reactive
{
    public interface IReadonlyVariable<T>
    {
        IDisposable Subscribe(Action<T, T> action);

        T Value { get; }
    }
}
