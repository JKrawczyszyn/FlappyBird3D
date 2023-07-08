using Game.Views;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class GameViewInstaller : MonoInstaller<GameViewInstaller>
    {
        [SerializeField]
        private CollectiblesView collectiblesView;

        [SerializeField]
        private CollisionResolver collisionResolver;

        public override void InstallBindings()
        {
            Container.BindInstance(collectiblesView);
            Container.BindInstance(collisionResolver);
        }
    }
}
