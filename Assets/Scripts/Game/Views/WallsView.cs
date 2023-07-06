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
        private GameController gameController;

        private string[] assetNames;

        private WallsConfig Config => gameConfig.wallsConfig;

        private void Start()
        {
            Initialize().Forget();
        }

        private async UniTaskVoid Initialize()
        {
            assetNames = assetsRepository.AssetNames(AssetTag.Walls);

            await assetsProvider.CacheReferences<Walls>(assetNames);

            gameController.OnGameStarted += StartMove;

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
            gameController.OnGameStarted -= StartMove;
        }
    }
}
