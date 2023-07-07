using Utilities.FSM;
using Zenject;

namespace Entry.Controllers
{
    public class FlowState : State
    {
        [Inject]
        protected StateMachine<FlowState> StateMachine { get; private set; }
    }
}
