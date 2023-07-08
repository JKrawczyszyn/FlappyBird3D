using Entry.Controllers;
using Entry.Services;
using UnityEngine;
using Utilities;
using Utilities.FSM;
using Zenject;

namespace Entry.Installers
{
    public class EntryInstaller : MonoInstaller<EntryInstaller>
    {
        [SerializeField]
        private Camera camera;

        [SerializeField]
        private Config config;

        [SerializeField]
        private AssetsRepository assetsRepository;

        public override void InstallBindings()
        {
            Container.BindInstance(camera).AsSingle();
            Container.BindInstance(config).AsSingle();
            Container.BindInstance(assetsRepository).AsSingle();

            Container.Bind<SceneLoader>().AsSingle();

            Container.BindInterfacesAndSelfTo<FlowController>().AsSingle();

            Container.BindAllDerivedInterfacesAndSelf<IService>(m => m.AsSingle().NonLazy());

            FSMInstaller<FlowState>.Install(Container);
        }
    }
}
