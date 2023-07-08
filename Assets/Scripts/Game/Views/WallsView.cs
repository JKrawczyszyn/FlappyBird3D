using Cysharp.Threading.Tasks;
using Entry.Models;
using Entry.Services;
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
        private AssetsService assetsService;

        [Inject]
        private Config config;

        [Inject]
        private AssetsRepository assetsRepository;

        private string[] assetNames;

        private WallsConfig Config => config.wallsConfig;

        [Inject]
        private async UniTaskVoid Construct()
        {
            assetNames = assetsRepository.AssetNames(AssetTag.Walls);

            await assetsService.CacheReferences(assetNames);

            loopView.Initialize(assetNames.GetRandom(), Config.interval, Config.loop, CreateWall);
        }

        private GameObject CreateWall(string name, Vector3 position, Transform parent) =>
            assetsService.Instantiate<Walls>(name, position, parent).gameObject;
    }
}
