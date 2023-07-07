using Utilities.FSM;
using Zenject;

namespace Game.Controllers
{
    public class GameplayController : IController
    {
        [Inject]
        private StateMachine<GameplayState> stateMachine;

        public void StartGame()
        {
            stateMachine.RequestTransition<CountdownState>();
            stateMachine.Start();
        }
    }
}
