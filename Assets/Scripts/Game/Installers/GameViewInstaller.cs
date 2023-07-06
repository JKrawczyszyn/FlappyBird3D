using Game.Views;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class GameViewInstaller : MonoInstaller<GameViewInstaller>
    {
        [SerializeField]
        private WallsView wallsView;

        public override void InstallBindings()
        {
            Container.BindInstance(wallsView);
        }
    }
}
