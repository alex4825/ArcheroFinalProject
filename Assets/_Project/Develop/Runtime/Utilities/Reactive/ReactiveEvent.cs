using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities.Reactive
{
    public class ReactiveEvent : IReadonlyEvent
    {
        private List<Subscriber> _subscribers = new();
        private List<Subscriber> _toAdd = new();
        private List<Subscriber> _toRemove = new();

        public IDisposable Subscribe(Action action)
        {
            Subscriber subscriber = new(action, Remove);
            _toAdd.Add(subscriber);
            return subscriber;
        }

        private void Remove(Subscriber subscriber) => _toRemove.Add(subscriber);

        public void Invoke()
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
                subscriber.Invoke();
        }
    }


    public class ReactiveEvent<T1> : IReadonlyEvent<T1>
    {
        private List<Subscriber<T1>> _subscribers = new();
        private List<Subscriber<T1>> _toAdd = new();
        private List<Subscriber<T1>> _toRemove = new();

        public IDisposable Subscribe(Action<T1> action)
        {
            Subscriber<T1> subscriber = new(action, Remove);
            _toAdd.Add(subscriber);
            return subscriber;
        }

        private void Remove(Subscriber<T1> subscriber) => _toRemove.Add(subscriber);

        public void Invoke(T1 arg)
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
                subscriber.Invoke(arg);
        }
    }


    public class ReactiveEvent<T1, T2> : IReadonlyEvent<T1, T2>
    {
        private List<Subscriber<T1, T2>> _subscribers = new();
        private List<Subscriber<T1, T2>> _toAdd = new();
        private List<Subscriber<T1, T2>> _toRemove = new();

        public IDisposable Subscribe(Action<T1, T2> action)
        {
            Subscriber<T1, T2> subscriber = new(action, Remove);
            _toAdd.Add(subscriber);
            return subscriber;
        }

        private void Remove(Subscriber<T1, T2> subscriber) => _toRemove.Add(subscriber);

        public void Invoke(T1 arg1, T2 arg2)
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
                subscriber.Invoke(arg1, arg2);
        }
    }
}
