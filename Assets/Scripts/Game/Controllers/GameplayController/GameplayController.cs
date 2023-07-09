using System;
using Game.Models;
using Utilities.States;
using Zenject;

namespace Game.Controllers
{
    public class GameplayController : IController, IDisposable
    {
        [Inject]
        private StateMachine<GameplayState> stateMachine;

        [Inject]
        private ScoreController scoreController;

        public GameplayState CurrentState => stateMachine.CurrentState;

        public void Initialize()
        {
            IdProvider.Reset();
        }

        public void Start()
        {
            stateMachine.Transition<CountdownState>();
        }

        public void LostGame()
        {
            stateMachine.Transition<LostGameState, int>(scoreController.Score);
        }

        public void Dispose()
        {
            stateMachine.Stop();
        }
    }
}
