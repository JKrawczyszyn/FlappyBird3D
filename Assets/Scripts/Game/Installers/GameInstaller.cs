using Game.Controllers;
using Game.Models;
using UnityEngine;
using Utilities;
using Utilities.FSM;
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

            Container.BindInterfacesAndSelfTo<MovingObjectsController<ObstacleModel>>().AsSingle();
            Container.BindInterfacesAndSelfTo<MovingObjectsController<CollectibleModel>>().AsSingle();

            FSMInstaller<GameplayState>.Install(Container);
        }
    }
}
