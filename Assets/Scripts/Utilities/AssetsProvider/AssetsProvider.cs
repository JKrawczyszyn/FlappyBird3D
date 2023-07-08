using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Utilities
{
    public class AssetsProvider
    {
        private readonly Dictionary<Type, IAssetsManager> managers = new();

        public async UniTask CacheReferences<T>(IEnumerable<string> names) where T : Component
        {
            var manager = GetAssetsManager<T>();

            await ((AssetsManager<T>)manager).CacheReferences(names);
        }

        private IAssetsManager GetAssetsManager<T>() where T : Component
        {
            var type = typeof(T);

            if (managers.TryGetValue(type, out IAssetsManager manager))
                return manager;

            manager = new AssetsManager<T>();

            managers.Add(type, manager);

            return manager;
        }

        public T Instantiate<T>(string name, Vector3 position, Transform parent) where T : Component
        {
            var type = typeof(T);

            if (managers.TryGetValue(type, out IAssetsManager manager))
                return ((AssetsManager<T>)manager).Instantiate(name, position, parent);

            Debug.LogError($"Manager not found for '{type}'.");

            return null;
        }

        public void Release<T>(T instance) where T : Component
        {
            var type = typeof(T);

            if (managers.TryGetValue(type, out IAssetsManager manager))
            {
                ((AssetsManager<T>)manager).Release(instance);

                return;
            }

            Debug.LogError($"Manager not found for '{type}'.");
        }

        public async UniTask WaitForCache(string[] names)
        {
            var cachedNames = new HashSet<string>();

            do
            {
                cachedNames.Clear();

                foreach ((Type _, IAssetsManager manager) in managers)
                    cachedNames.UnionWith(manager.CachedNames);

                if (cachedNames.IsSupersetOf(names))
                    return;

                await UniTask.Delay(97);

            } while (!cachedNames.IsSupersetOf(names));
        }

        public void ClearPools()
        {
            foreach ((Type _, IAssetsManager manager) in managers)
                manager.ClearPools();
        }
    }
}
