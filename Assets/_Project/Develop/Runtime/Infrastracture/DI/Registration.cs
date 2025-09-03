using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Infrastracture.DI
{
    public class Registration : IRegistrationsOptions
    {
        private Func<DIContainer, object> _creator;
        private object _cachedInstance;
        private readonly List<IUpdatable> _updatables;

        public Registration(Func<DIContainer, object> creator, List<IUpdatable> updatables)
        {
            _creator = creator;
            _updatables = updatables;
        }

        public bool IsNonLazy { get; private set; }

        public object CreateInstanceFrom(DIContainer container)
        {
            if (_cachedInstance != null)
                return _cachedInstance;

            if (_creator == null)
                throw new InvalidOperationException("Not has instance or creator");

            _cachedInstance = _creator.Invoke(container);

            if (_cachedInstance is IUpdatable updatable)
                _updatables.Add(updatable);

            return _cachedInstance;
        }

        public void NonLazy() => IsNonLazy = true;

        public void OnInitialize()
        {
            if (_cachedInstance != null)
                if (_cachedInstance is IInitializable initializable)
                    initializable.Initialize();
        }

        public void OnDispose()
        {
            if (_cachedInstance == null)
                return;

            if (_cachedInstance is IUpdatable updatable)
                _updatables.Remove(updatable);

            if (_cachedInstance is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
