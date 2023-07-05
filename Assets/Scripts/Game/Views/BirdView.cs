using Cysharp.Threading.Tasks;
using Fp.Utilities.Assets;
using UnityEngine;
using Zenject;

namespace Fp.Game.Views
{
    public class BirdView : MonoBehaviour
    {
        [SerializeField]
        private SceneContext context;

        [SerializeField]
        private Transform container;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsProvider assetsProvider;

        [Inject]
        private GameConfig gameConfig;

        private string[] assetNames;

        [Inject]
        public async UniTaskVoid Construct()
        {
            assetNames = assetsRepository.AssetNames(AssetTag.Bird);

            await assetsProvider.CacheReferences<Bird>(assetNames);

            var instance
                = assetsProvider.Instantiate<Bird>(assetNames.GetRandom(), gameConfig.birdConfig.startPosition,
                                                   container);

            context.Container.Inject(instance);
        }
    }
}
