using Utilities;
using Zenject;

namespace Entry.Controllers
{
    public abstract class GameFlowState : IState
    {
        [Inject]
        protected GameFlowController Controller;

        public abstract void Perform();
    }
}
