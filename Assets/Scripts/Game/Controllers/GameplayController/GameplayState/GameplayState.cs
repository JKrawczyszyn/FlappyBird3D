using Utilities.FSM;
using Zenject;

namespace Game.Controllers
{
    public class GameplayState : State
    {
        [Inject]
        protected StateMachine<GameplayState> StateMachine { get; private set; }
    }
}
