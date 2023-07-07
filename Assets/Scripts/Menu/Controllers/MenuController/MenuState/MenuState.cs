using Utilities.FSM;
using Zenject;

namespace Menu.Controllers
{
    public class MenuState : State
    {
        [Inject]
        protected StateMachine<MenuState> StateMachine { get; private set; }
    }
}
