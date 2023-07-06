using Menu.Controllers;
using Utilities;
using Zenject;

namespace Menu.Installers
{
    public class MenuInstaller : MonoInstaller<MenuInstaller>
    {
        public override void InstallBindings()
        {
            BindMenuController();
        }

        private void BindMenuController()
        {
            Container.BindInterfacesAndSelfTo<MenuController>().AsSingle();
            Container.BindInterfacesAndSelfTo<StateMachine<MenuState>>().AsSingle();
            Container.BindAllDerivedInterfacesAndSelf<MenuState>(c => c.AsSingle());
        }
    }
}
