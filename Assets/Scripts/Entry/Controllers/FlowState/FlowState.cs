using Utilities.States;
using Zenject;

namespace Entry.Controllers
{
    public class FlowState : State
    {
        [Inject]
        protected StateMachine<FlowState> StateMachine { get; private set; }
    }
}
