using Entry.Controllers;
using Entry.Views;
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
        private AssetsRepository assetsRepository;

        [SerializeField]
        private LoadingBlendView loadingBlendView;

        public override void InstallBindings()
        {
            Container.BindInstance(camera);
            Container.BindInstance(assetsRepository);
            Container.BindInstance(loadingBlendView);

            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<AssetsProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<FlowController>().AsSingle();

            FSMInstaller<FlowState>.Install(Container);
        }
    }
}
