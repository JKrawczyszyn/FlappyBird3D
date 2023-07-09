using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using Utilities;
using Object = UnityEngine.Object;

namespace Entry.Services
{
    public class AssetsService : IService, IDisposable
    {
        private readonly Dictionary<AssetReference, IObjectPool<GameObject>> referenceToPool = new();
        private readonly Dictionary<string, AssetReference> nameToReference = new();

        private IEnumerable<string> CachedNames => nameToReference.Keys;

        public async UniTask WaitForCache(string[] names)
        {
            do
            {
                await UniTask.Delay(97);
            } while (!CachedNames.ToHashSet().IsSupersetOf(names));
        }

        public async UniTask CacheReferences(IEnumerable<string> names)
        {
            IEnumerable<UniTask> tasks = names.Select(CacheReference);
            await UniTask.WhenAll(tasks);
        }

        private async UniTask CacheReference(string name)
        {
            await CacheReference(new AssetReference(name));
        }

        public T Instantiate<T>(string name, Vector3 position, Transform parent) where T : Component
        {
            GameObject go = Instantiate(name, position, parent);

            bool success = go.TryGetComponent(out T component);
            Assert.IsTrue(success, $"No component of type '{typeof(T)}' found in '{go.name}'.");

            return component;
        }

        private GameObject Instantiate(string name, Vector3 position, Transform parent)
        {
            if (nameToReference.TryGetValue(name, out AssetReference reference))
                return Instantiate(reference, position, parent);

            Debug.Log($"Reference not found for '{name}'. Cache object first.");

            return null;
        }

        private GameObject Instantiate(AssetReference reference, Vector3 position, Transform parent)
        {
            if (!reference.IsValid())
            {
                Debug.Log($"Invalid reference '{reference}'.");

                return null;
            }

            if (!referenceToPool.TryGetValue(reference, out IObjectPool<GameObject> pool))
            {
                Debug.Log($"Pool not found for '{reference}'.");

                return null;
            }

            GameObject instance = pool.Get();

            if (instance == null)
            {
                Debug.Log($"GameObject not found for '{reference}'.");

                pool.Release(instance);

                return null;
            }

            instance.transform.SetParent(parent, false);
            instance.transform.SetLocalPositionAndRotation(position, Quaternion.identity);

            return instance;
        }

        public void Release<T>(T instance) where T : Component
        {
            Release(instance.gameObject);
        }

        private void Release(GameObject instance)
        {
            if (instance == null || instance.gameObject == null || !instance.gameObject.activeSelf)
                return;

            string name = NormalizeName(instance.name);

            if (!nameToReference.TryGetValue(name, out AssetReference reference))
            {
                Debug.Log($"Prefab not found for '{name}'.");

                return;
            }

            if (!referenceToPool.TryGetValue(reference, out IObjectPool<GameObject> pool))
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

        private IObjectPool<GameObject> CreatePool(AssetReference reference)
        {
            var pool = new ObjectPool<GameObject>(onCreate, onGet, onRelease, onDestroy);

            GameObject onCreate()
            {
                Assert.IsTrue(referenceToPool.ContainsKey(reference), $"Reference '{reference}' pool not found.");
                Assert.IsTrue(reference.IsDone, $"Reference '{reference}' not loaded.");

                var referenceAsset = reference.Asset as GameObject;
                Assert.IsNotNull(referenceAsset, $"Reference '{reference}' is not a GameObject.");

                GameObject go = Object.Instantiate(referenceAsset);

                Assert.IsNotNull(go, $"Can't instantiate GameObject from '{referenceAsset}'.");

                go.name = NormalizeName(go.name);
                go.transform.localScale = referenceAsset.transform.localScale;

                return go;
            }

            void onGet(GameObject e)
            {
                e.gameObject.SetActive(true);
            }

            void onRelease(GameObject e)
            {
                e.gameObject.SetActive(false);
            }

            void onDestroy(GameObject e)
            {
                Object.Destroy(e.gameObject);
            }

            return pool;
        }

        public void ClearPools()
        {
            foreach (IObjectPool<GameObject> pool in referenceToPool.Values)
                pool.Clear();
        }

        public void Dispose()
        {
            foreach (AssetReference reference in referenceToPool.Keys)
            {
                if (reference.IsValid())
                    reference.ReleaseAsset();
            }
        }
    }
}
