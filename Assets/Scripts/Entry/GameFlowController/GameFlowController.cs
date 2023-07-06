using Utilities;
using Zenject;

namespace Entry.Controllers
{
    public class GameFlowController
    {
        [Inject]
        private readonly StateMachine<GameFlowState> stateMachine;

        public void LostGame()
        {
            stateMachine.Push<LostGameState>();
            stateMachine.Perform();
        }

        public void StartGame()
        {
            stateMachine.Push<PlayGameState>();
            stateMachine.Perform();
        }

        public void Back()
        {
            stateMachine.Pop();
            stateMachine.Perform();
        }
    }
}
