using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Infrastracture.ServiceLocator
{
    public class ServiceLocator
    {
        private readonly Dictionary<Type, object> _services = new();

        public void AddService<T>(T service) => _services.Add(typeof(T), service);

        public T GetService<T>() => (T)_services[typeof(T)];
    }
}
