using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Pool;
// using Zenject;
using Object = UnityEngine.Object;

public class AssetManager : IDisposable// where T : Object // where T : MonoBehaviour
{
    private readonly Dictionary<AssetReference, IObjectPool<GameObject>> referenceToPool = new();
    private readonly Dictionary<string, AssetReference> nameToReference = new();

    // [Inject]
    // private readonly DiContainer container;

    // public async UniTask CacheReferences(IEnumerable<string> names)
    // {
    //     var tasks = names.Select(CacheReference);
    //     await UniTask.WhenAll(tasks);
    // }

    public async UniTask CacheReference(string name)
    {
        await CacheReference(new AssetReference(name));
    }

    // public async UniTask CacheReferences(AssetReference[] references)
    // {
    //     var tasks = references.Select(CacheReference);
    //     await UniTask.WhenAll(tasks);
    // }

    public GameObject Instantiate(string name, Vector3 position, Transform parent)
    {
        // var asset = GetAssetName(name);

        if (!nameToReference.ContainsKey(name))
        {
            Debug.Log($"Reference not found for '{name}'. Cache object first");

            return null;
        }

        return Instantiate(nameToReference[name], position, parent);
    }

    // private string GetAssetName(string name)
    // {
    //     int index = name.LastIndexOf('/') + 1;
    //
    //     return name[index..];
    // }

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

        var instance = pool.Get();

        if (instance == null)
        {
            Debug.Log($"Component '{typeof(GameObject)}' not found for '{reference}'.");

            pool.Release(instance);

            return null;
        }

        instance.transform.SetParent(parent, false);
        instance.transform.SetPositionAndRotation(position, Quaternion.identity);

        return instance;
    }

    public void Release(GameObject instance)
    {
        if (instance == null)// || instance.gameObject == null || !instance.gameObject.activeSelf)
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

    // public T GetPrefab(AssetReference reference)
    // {
    //     var referenceAsset = reference.Asset as GameObject;
    //     Assert.IsNotNull(referenceAsset, $"Reference '{reference}' is not a GameObject.");
    //
    //     bool success = referenceAsset.TryGetComponent(out T component);
    //     Assert.IsTrue(success, $"No '{typeof(T)}' component found in '{referenceAsset}'.");
    //
    //     return component;
    // }

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

        if (referenceToPool.ContainsKey(reference))
        {
            Assert.IsTrue(nameToReference.ContainsKey(reference.Asset.name),
                          $"namesToReference doesn't contain '{reference}'.");

            Assert.IsTrue(reference.IsDone, $"Reference '{reference}' not loaded.");

            return;
        }

        await LoadReference(reference);

        referenceToPool.Add(reference, CreatePool(reference));

        if (!nameToReference.ContainsKey(reference.Asset.name))
            nameToReference.Add(reference.Asset.name, reference);
    }

    private async UniTask LoadReference(AssetReference reference)
    {
        GameObject go;

        if (reference.Asset != null)
            go = reference.Asset as GameObject;
        else
            go = await reference.LoadAssetAsync<GameObject>().ToTask();

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

            // bool success = go.TryGetComponent(out T component);
            // if (success)
            //     return component;
            // Assert.IsTrue(success, $"No BoardElement component found in '{go}'.");

            // container.Inject(component);

            return go;
        }

        void onGet(GameObject e)
        {
            e.SetActive(true);
        }

        void onRelease(GameObject e)
        {
            e.SetActive(false);
        }

        void onDestroy(GameObject e)
        {
            Object.Destroy(e);
        }

        return pool;
    }

    public void Dispose()
    {
        if (referenceToPool == null)
            return;

        foreach (AssetReference reference in referenceToPool.Keys)
        {
            if (reference.IsValid())
                reference.ReleaseAsset();
        }
    }
}
