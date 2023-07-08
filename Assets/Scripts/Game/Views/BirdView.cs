using Cysharp.Threading.Tasks;
using Entry.Models;
using Entry.Services;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class BirdView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private Context context;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsService assetsService;

        [Inject]
        private GameConfig gameConfig;

        private string[] assetNames;

        private Bird bird;

        [Inject]
        private async UniTaskVoid Construct()
        {
            assetNames = assetsRepository.AssetNames(AssetTag.Bird);

            await assetsService.CacheReferences<Bird>(assetNames);

            bird = assetsService.Instantiate<Bird>(assetNames.GetRandom(), gameConfig.birdConfig.startPosition,
                                                    container);

            context.Container.Inject(bird);
        }
    }
}
