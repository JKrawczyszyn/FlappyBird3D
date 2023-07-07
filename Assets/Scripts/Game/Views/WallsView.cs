using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class WallsView : MonoBehaviour
    {
        [SerializeField]
        private InfiniteLoopView loopView;

        [Inject]
        private AssetsProvider assetsProvider;

        [Inject]
        private GameConfig gameConfig;

        [Inject]
        private AssetsRepository assetsRepository;

        private string[] assetNames;

        private WallsConfig Config => gameConfig.wallsConfig;

        [Inject]
        private async UniTaskVoid Construct()
        {
            assetNames = assetsRepository.AssetNames(AssetTag.Walls);

            await assetsProvider.CacheReferences<Walls>(assetNames);

            loopView.Initialize(assetNames.GetRandom(), Config.interval, Config.loop, CreateWall);
        }

        private GameObject CreateWall(string name, Vector3 position, Transform parent) =>
            assetsProvider.Instantiate<Walls>(name, position, parent).gameObject;
    }
}
