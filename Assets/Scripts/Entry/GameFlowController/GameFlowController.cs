using System;
using Utilities.FSM;
using Zenject;

namespace Entry.Controllers
{
    public class GameFlowController : IDisposable
    {
        [Inject]
        private SceneLoader sceneLoader;

        [Inject]
        private readonly StateMachine<FlowState> stateMachine;

        public void Initialize()
        {
            stateMachine.RequestTransition<LoadMenuState>();
            stateMachine.Start();
        }

        public void LoadGame()
        {
            stateMachine.RequestTransition<LoadGameState>();
            stateMachine.Start();
        }

        public void LostGame(int score)
        {
            stateMachine.RequestTransition<LostGameState>();
            stateMachine.Start();
        }

        public void Dispose()
        {
            stateMachine.Stop();
        }
    }
}
