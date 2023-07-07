using Menu.Controllers;
using Utilities.FSM;
using Zenject;

namespace Menu.Installers
{
    public class MenuInstaller : MonoInstaller<MenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuController>().AsSingle();

            FSMInstaller<MenuState>.Install(Container);
        }
    }
}
