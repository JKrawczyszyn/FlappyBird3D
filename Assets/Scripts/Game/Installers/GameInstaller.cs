using Game.Controllers;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField]
        private GameConfig gameConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(gameConfig).AsSingle();

            Container.Bind<InputControls>().AsSingle();

            Container.BindAllDerivedInterfacesAndSelf<IController>(m => m.AsSingle().NonLazy());
        }
    }
}
