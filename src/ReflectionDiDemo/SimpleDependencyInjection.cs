using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectionDiDemo
{
    public enum CreationRules
    {
        Transient,
        Static,
    }

    public class TypeContainer
    {
        public CreationRules Rule { get; set; }
        public object Instance { get; set; }
        public Type Implementation { get; set; }
    }

    public class SimpleDependencyInjection : IDisposable
    {

        private readonly Dictionary<Type, TypeContainer> _container;

        public SimpleDependencyInjection()
        {
            _container = new Dictionary<Type, TypeContainer>();
        }

        public Type[] GetAllTypes()
        {
            return _container.Keys.ToArray();
        }

        public SimpleDependencyInjection RegisterType<T>()
        {
            return RegisterType<T>(CreationRules.Transient);
        }

        public SimpleDependencyInjection RegisterType<T>(CreationRules creationRules)
        {
            _container.Add(typeof(T), new TypeContainer { Rule = creationRules });
            return this;
        }

        public SimpleDependencyInjection RegisterType<TInterface, TImplementation>(CreationRules creationRules)
            where TImplementation : TInterface
        {
            _container.Add(typeof(TInterface), new TypeContainer { Rule = creationRules, Implementation = typeof(TImplementation) });
            return this;
        }

        public SimpleDependencyInjection RegisterInterface<TInterface>(CreationRules creationRules)
        {
            var tInterface = typeof(TInterface);

            var implementation = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => tInterface.IsAssignableFrom(p))
                .Where(p => !p.IsInterface)
                .FirstOrDefault();

            if (implementation == null)
            {
                throw new Exception("No implementation found");
            }

            _container.Add(tInterface, new TypeContainer { Rule = creationRules, Implementation = implementation });
            return this;
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        private object Get(Type T)
        {
            var typeContainer = _container[T];

            if (typeContainer.Rule == CreationRules.Static)
            {
                if (typeContainer.Instance is null)
                {
                    if (typeContainer.Implementation != null)
                    {
                        typeContainer.Instance = CreateInstance(typeContainer.Implementation);
                    }
                    else
                    {
                        typeContainer.Instance = CreateInstance(T);
                    }
                }

                return typeContainer.Instance;
            }

            if(typeContainer.Implementation != null)
            {
                return CreateInstance(typeContainer.Implementation);
            }

            return CreateInstance(T);
        }

        private object CreateInstance(Type t)
        {
            if (t.IsInterface)
            {
                t = _container[t].Implementation;
            }

            var ctors = t.GetConstructors();

            ConstructorInfo constructorInfo = null;
            foreach (var ctor in ctors.OrderBy(a => a.GetParameters().Count()))
            {
                var arguments = ctor.GetParameters();
                if (!arguments.Select(a => a.ParameterType).Except(_container.Keys).Any())
                {
                    constructorInfo = ctor;
                    break;
                }
            }

            if (constructorInfo == null)
            {
                throw new Exception("Missing arguments");
            }

            var args = constructorInfo.GetParameters().Select(a => Get(a.ParameterType)).ToArray();

            return Activator.CreateInstance(t, args);
        }

        public void Dispose()
        {
            Dispose(fromDispose: true);
        }

        ~SimpleDependencyInjection()
        {
            Dispose(fromDispose: false);
        }

        private void Dispose(bool fromDispose)
        {
            foreach (var item in _container.Where( c => c.Value.Instance != null).Select(c => c.Value.Instance))
            {
                var dispose = item as IDisposable;
                if(dispose != null)
                {
                    dispose.Dispose();
                }
            }
        }
    }
}