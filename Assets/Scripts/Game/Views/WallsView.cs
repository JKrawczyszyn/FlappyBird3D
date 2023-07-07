using Cysharp.Threading.Tasks;
using Game.Controllers;
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

        [Inject]
        private PlayState playState;

        private string[] assetNames;

        private WallsConfig Config => gameConfig.wallsConfig;

        [Inject]
        private async UniTaskVoid Construct()
        {
            playState.OnStart += StartMove;

            assetNames = assetsRepository.AssetNames(AssetTag.Walls);

            await assetsProvider.CacheReferences<Walls>(assetNames);

            loopView.Initialize(assetNames.GetRandom(), Config.interval, Config.loop, CreateWall);
        }

        private GameObject CreateWall(string name, Vector3 position, Transform parent) =>
            assetsProvider.Instantiate<Walls>(name, position, parent).gameObject;

        private void StartMove()
        {
            loopView.StartMove();
        }

        private void OnDestroy()
        {
            playState.OnStart -= StartMove;
        }
    }
}
