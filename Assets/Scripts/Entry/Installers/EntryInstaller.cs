using Entry.Controllers;
using UnityEngine;
using Utilities;
using Zenject;

namespace Entry.Installers
{
    public class EntryInstaller : MonoInstaller<EntryInstaller>
    {
        [SerializeField]
        private Camera camera;

        [SerializeField]
        private AssetsRepository assetsRepository;

        public override void InstallBindings()
        {
            Container.BindInstance(camera);
            Container.BindInstance(assetsRepository);

            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<AssetsProvider>().AsSingle();

            BindGameFlowController();
        }

        private void BindGameFlowController()
        {
            Container.BindInterfacesAndSelfTo<GameFlowController>().AsSingle();
            Container.BindInterfacesAndSelfTo<StateMachine<GameFlowState>>().AsSingle();
            Container.BindAllDerivedInterfacesAndSelf<GameFlowState>(c => c.AsSingle());
        }
    }
}
