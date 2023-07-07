using Cysharp.Threading.Tasks;
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
        private AssetsProvider assetsProvider;

        [Inject]
        private GameConfig gameConfig;

        private string[] assetNames;

        private Bird bird;

        [Inject]
        private async UniTaskVoid Construct()
        {
            assetNames = assetsRepository.AssetNames(AssetTag.Bird);

            await assetsProvider.CacheReferences<Bird>(assetNames);

            bird = assetsProvider.Instantiate<Bird>(assetNames.GetRandom(), gameConfig.birdConfig.startPosition,
                                                    container);

            context.Container.Inject(bird);
        }
    }
}
