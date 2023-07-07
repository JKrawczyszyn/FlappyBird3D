using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Utilities
{
    internal interface IAssetsManager
    {
        IEnumerable<string> CachedNames { get; }
        void ClearPools();
    }

    internal class AssetsManager<T> : IDisposable, IAssetsManager where T : Component
    {
        private readonly Dictionary<AssetReference, IObjectPool<T>> referenceToPool = new();
        private readonly Dictionary<string, AssetReference> nameToReference = new();

        public IEnumerable<string> CachedNames => nameToReference.Keys;

        public async UniTask CacheReferences(IEnumerable<string> names)
        {
            var tasks = names.Select(CacheReference);
            await UniTask.WhenAll(tasks);
        }

        private async UniTask CacheReference(string name)
        {
            await CacheReference(new AssetReference(name));
        }

        public T Instantiate(string name, Vector3 position, Transform parent)
        {
            if (nameToReference.TryGetValue(name, out var reference))
                return Instantiate(reference, position, parent);

            Debug.Log($"Reference not found for '{name}'. Cache object first.");

            return null;
        }

        private T Instantiate(AssetReference reference, Vector3 position, Transform parent)
        {
            if (!reference.IsValid())
            {
                Debug.Log($"Invalid reference '{reference}'.");

                return null;
            }

            if (!referenceToPool.TryGetValue(reference, out IObjectPool<T> pool))
            {
                Debug.Log($"Pool not found for '{reference}'.");

                return null;
            }

            var instance = pool.Get();

            if (instance == null)
            {
                Debug.Log($"Component '{typeof(T)}' not found for '{reference}'.");

                pool.Release(instance);

                return null;
            }

            instance.transform.SetParent(parent, false);
            instance.transform.SetPositionAndRotation(position, Quaternion.identity);

            return instance;
        }

        public void Release(T instance)
        {
            if (instance == null || instance.gameObject == null || !instance.gameObject.activeSelf)
                return;

            string name = NormalizeName(instance.name);

            if (!nameToReference.TryGetValue(name, out AssetReference reference))
            {
                Debug.Log($"Prefab not found for '{name}'.");

                return;
            }

            if (!referenceToPool.TryGetValue(reference, out IObjectPool<T> pool))
            {
                Debug.Log($"Pool not found for '{name}'.");

                return;
            }

            pool.Release(instance);
        }

        private string NormalizeName(string name) => name.Replace("(Clone)", string.Empty).Trim();

        private async UniTask CacheReference(AssetReference reference)
        {
            if (reference == null || reference.AssetGUID.IsNullOrWhitespace())
                return;

            if (reference.LoadingInProgress())
            {
                await reference.OperationHandle.Task;

                return;
            }

            if (nameToReference.ContainsKey(reference.AssetGUID))
                return;

            if (referenceToPool.ContainsKey(reference))
            {
                Assert.IsTrue(nameToReference.ContainsKey(reference.Asset.name),
                              $"namesToReference doesn't contain '{reference}'.");

                Assert.IsTrue(reference.IsDone, $"Reference '{reference}' not loaded.");

                return;
            }

            await LoadReference(reference);

            Assert.AreEqual(reference.AssetGUID, reference.Asset.name, "AssetGUID != name");

            referenceToPool.Add(reference, CreatePool(reference));
            nameToReference.Add(reference.Asset.name, reference);
        }

        private async UniTask LoadReference(AssetReference reference)
        {
            GameObject go;

            if (reference.Asset != null)
                go = reference.Asset as GameObject;
            else
                go = await reference.LoadAssetAsync<GameObject>();

            Assert.IsFalse(go == null, $"Can't load '{reference}'.");
        }

        private IObjectPool<T> CreatePool(AssetReference reference)
        {
            var pool = new ObjectPool<T>(onCreate, onGet, onRelease, onDestroy);

            T onCreate()
            {
                Assert.IsTrue(referenceToPool.ContainsKey(reference), $"Reference '{reference}' pool not found.");
                Assert.IsTrue(reference.IsDone, $"Reference '{reference}' not loaded.");

                var referenceAsset = reference.Asset as GameObject;
                Assert.IsNotNull(referenceAsset, $"Reference '{reference}' is not a GameObject.");

                var go = Object.Instantiate(referenceAsset);

                Assert.IsNotNull(go, $"Can't instantiate GameObject from '{referenceAsset}'.");

                var success = go.TryGetComponent(out T component);
                Assert.IsTrue(success, $"No component found in '{go}'.");

                go.name = NormalizeName(go.name);
                go.transform.localScale = referenceAsset.transform.localScale;

                return component;
            }

            void onGet(T e)
            {
                e.gameObject.SetActive(true);
            }

            void onRelease(T e)
            {
                e.gameObject.SetActive(false);
            }

            void onDestroy(T e)
            {
                Object.Destroy(e.gameObject);
            }

            return pool;
        }

        public void ClearPools()
        {
            foreach (var pool in referenceToPool.Values)
                pool.Clear();
        }

        public void Dispose()
        {
            foreach (var reference in referenceToPool.Keys)
            {
                if (reference.IsValid())
                    reference.ReleaseAsset();
            }
        }
    }
}
