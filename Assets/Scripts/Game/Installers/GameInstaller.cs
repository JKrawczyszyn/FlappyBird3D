using Fp.Game.Controllers;
using UnityEngine;
using Zenject;

namespace Fp.Game.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField]
        private GameConfig gameConfig;

        [SerializeField]
        private AssetsRepository assetsRepository;

        public override void InstallBindings()
        {
            SetGlobalConfigs();

            Container.BindInstance(gameConfig);
            Container.BindInstance(assetsRepository);
            Container.BindInstance(new GameControls());

            Container.BindInterfacesAndSelfTo<BirdController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpeedController>().AsSingle();
        }

        private void SetGlobalConfigs()
        {
            Application.targetFrameRate = gameConfig.frameRate;
            Physics.gravity = Vector3.down * gameConfig.gravity;
        }
    }
}
