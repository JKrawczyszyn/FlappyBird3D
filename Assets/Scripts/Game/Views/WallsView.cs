using Cysharp.Threading.Tasks;
using Fp.Game.Controllers;
using UnityEngine;
using Fp.Utilities.Assets;
using Zenject;

namespace Fp.Game.Views
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

        [Inject]
        public async UniTaskVoid Construct()
        {
            assetNames = assetsRepository.AssetNames(AssetTag.Walls);

            await assetsProvider.CacheReferences<Walls>(assetNames);

            gameController.OnGameStarted += StartMove;

            loopView.Init(assetNames.GetRandom(), Config.interval, Config.loop, CreateWall);
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
