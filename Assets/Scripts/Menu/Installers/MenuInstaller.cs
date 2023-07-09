using Menu.Controllers;
using Menu.Views;
using Utilities.States;
using Zenject;

namespace Menu.Installers
{
    public class MenuInstaller : MonoInstaller<MenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuController>().AsSingle();
            Container.Bind<PanelFactory>().AsSingle();

            FSMInstaller<MenuState>.Install(Container);
        }
    }
}
