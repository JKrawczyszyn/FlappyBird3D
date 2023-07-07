using Zenject;

namespace Utilities.FSM
{
    public class FSMInstaller<T> : Installer<FSMInstaller<T>> where T : State
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<StateMachine<T>>().AsSingle();
            Container.BindAllDerivedInterfacesAndSelf<T>(c => c.AsSingle());
        }
    }
}
