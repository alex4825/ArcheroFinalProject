using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities.Reactive
{
    public class ReactiveVariable<T> : IReadonlyVariable<T> where T : IEquatable<T>
    {
        private List<Subscriber<T, T>> _subscribers = new();
        private List<Subscriber<T, T>> _toAdd = new();
        private List<Subscriber<T, T>> _toRemove = new();

        private T _value;

        public ReactiveVariable(T value) => _value = value;

        public ReactiveVariable() => _value = default(T);

        public T Value
        {
            get => _value;

            set
            {
                T oldValue = _value;

                _value = value;

                if (_value.Equals(oldValue) == false)
                    Invoke(oldValue, _value);
            }
        }

        public IDisposable Subscribe(Action<T, T> action)
        {
            Subscriber<T, T> subscriber = new(action, Remove);
            _toAdd.Add(subscriber);
            return subscriber;
        }

        private void Remove(Subscriber<T, T> subscriber) => _toRemove.Add(subscriber);

        private void Invoke(T oldValue, T newValue)
        {
            if (_toAdd.Count > 0)
            {
                _subscribers.AddRange(_toAdd);
                _toAdd.Clear();
            }

            if (_toRemove.Count > 0)
            {
                foreach (var subscriber in _toRemove)
                    _subscribers.Remove(subscriber);

                _toRemove.Clear();
            }

            foreach (var subscriber in _subscribers)
                subscriber.Invoke(oldValue, newValue);
        }
    }
}
