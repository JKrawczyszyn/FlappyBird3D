using System;
using Entry.Models;
using Utilities.FSM;
using Zenject;

namespace Entry.Controllers
{
    public class FlowController : IDisposable
    {
        [Inject]
        private readonly StateMachine<FlowState> stateMachine;

        public void Initialize()
        {
            stateMachine.Transition<LoadMenuState>();
        }

        public void LoadMenu(Score score)
        {
            stateMachine.Transition<LoadMenuState, Score>(score);
        }

        public void LoadGame()
        {
            stateMachine.Transition<LoadGameState>();
        }

        public void Dispose()
        {
            stateMachine.Stop();
        }
    }
}