using Fp.Game.Views;
using UnityEngine;
using Fp.Utilities.Assets;
using Zenject;

namespace Fp.Game.Installers
{
    public class GameViewInstaller : MonoInstaller<GameViewInstaller>
    {
        [SerializeField]
        private Camera camera;

        [SerializeField]
        private WallsView wallsView;

        public override void InstallBindings()
        {
            Container.BindInstance(camera);
            Container.BindInstance(wallsView);

            Container.Bind<AssetsProvider>().AsSingle();
        }
    }
}
