using Game.Controllers;
using Game.Models;
using Utilities;
using Utilities.States;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindAllDerivedInterfacesAndSelf<IController>(m => m.AsSingle().NonLazy());

            Container.BindInterfacesAndSelfTo<MovingObjectsController<ObstacleModel>>().AsSingle();
            Container.BindInterfacesAndSelfTo<MovingObjectsController<CollectibleModel>>().AsSingle();

            FSMInstaller<GameplayState>.Install(Container);
        }
    }
}
