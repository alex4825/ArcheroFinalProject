using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Infrastracture.DI
{
    public class DIContainer
    {
        private readonly Dictionary<Type, Registration> _container = new();

        public void RegisterAsSingle<T>(Func<DIContainer, T> creator)
        {
            Registration registration = new Registration(container => creator.Invoke(container));

            _container.Add(typeof(T), registration);
        }

        public T Resolve<T>()
        {
            if (_container.TryGetValue(typeof(T), out Registration registration))
                return (T)registration.CreateInstanceFrom(this);

            throw new InvalidOperationException($"Registration for {typeof(T)} not exists");
        }
    }
}
