using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Utilities
{
    public static class AddressableExtensions
    {
        public static bool LoadingInProgress(this AssetReference reference) => reference.OperationHandle.IsValid()
                                                                               && reference.OperationHandle.Status
                                                                               == AsyncOperationStatus.None;
    }
}
