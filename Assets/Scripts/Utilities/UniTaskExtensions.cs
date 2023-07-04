using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class UniTaskExtensions
{
    public static async UniTask<T> ToTask<T>(this AsyncOperationHandle<T> handle)
    {
        await handle;
        return handle.Result;
    }
}
