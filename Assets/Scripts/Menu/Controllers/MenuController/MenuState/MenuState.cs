using Utilities.States;
using Zenject;

namespace Menu.Controllers
{
    public class MenuState : State
    {
        [Inject]
        protected StateMachine<MenuState> StateMachine { get; private set; }
    }
}
