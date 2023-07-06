using System;
using System.Linq;
using System.Reflection;
using ModestTree;
using Zenject;

namespace Utilities
{
    public static class ZenjectExtensions
    {
        public static void BindAllDerivedInterfacesAndSelf<T>(this DiContainer container, Action<FromBinder> scope)
        {
            var assembly = Assembly.GetAssembly(typeof(T));

            var types = assembly.GetTypes()
                .Where(t => t.DerivesFrom<T>() && !t.IsAbstract && t.IsClass);

            foreach (var type in types)
                scope(container.BindInterfacesAndSelfTo(type));
        }
    }
}
