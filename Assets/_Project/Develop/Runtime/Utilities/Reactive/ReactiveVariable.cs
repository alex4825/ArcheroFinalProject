using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities.Reactive
{
    public class ReactiveVariable<T> : IReadonlyVariable<T> where T : IEquatable<T>
    {
        private List<Subscriber<T, T>> _subscribers = new();

        private T _value;

        public ReactiveVariable(T value) => Value = value;

        public ReactiveVariable() => Value = default(T);

        public T Value
        {
            get => _value;

            set
            {
                T oldValue = _value;

                _value = value;

                if (_value.Equals(oldValue) == false)
                    foreach (var subscriber in _subscribers)
                        subscriber?.Invoke(oldValue, _value);
            }
        }

        public IDisposable Subscribe(Action<T, T> action)
        {
            Subscriber<T, T> subscriber = new(action, Remove);
            _subscribers.Add(subscriber);
            return subscriber;
        }

        private void Remove(Subscriber<T, T> subscriber) => _subscribers.Remove(subscriber);
    }
}
