using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WallsView : MonoBehaviour
{
    private const string assetName = "Walls";

    [SerializeField]
    private InfiniteLoopView wallsLoopView;

    [Inject]
    private WallsAssetManager wallsManager;

    [Inject]
    public async UniTaskVoid Construct()
    {
        await wallsManager.CacheReference(assetName);

        wallsLoopView.Init(assetName, 100, 3, wallsManager);

        wallsLoopView.Speed = 10;
    }
}
