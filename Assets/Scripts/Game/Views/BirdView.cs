using Cysharp.Threading.Tasks;
using Entry;
using Entry.Models;
using Entry.Services;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class BirdView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private RandomService randomService;

        [Inject]
        private Context context;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsService assetsService;

        [Inject]
        private Config config;

        private string[] assetNames;

        private Bird bird;

        [Inject]
        private async UniTaskVoid Construct()
        {
            assetNames = assetsRepository.AssetNames(AssetTag.Bird);

            await assetsService.CacheReferences(assetNames);

            var assetName = randomService.GetRandom(assetNames);
            bird = assetsService.Instantiate<Bird>(assetName, config.birdConfig.startPosition, container);

            context.Container.Inject(bird);
        }
    }
}
