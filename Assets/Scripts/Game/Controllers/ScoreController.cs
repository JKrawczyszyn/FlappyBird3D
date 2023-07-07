using System;
using Zenject;

namespace Game.Controllers
{
    public class ScoreController : IController
    {
        public event Action<int> OnScoreChanged;

        [Inject]
        private GameplayController gameplayController;

        public int Score { get; private set; }

        [Inject]
        private void Construct()
        {
            Score = 0;
        }

        public void AddScore()
        {
            if (gameplayController.CurrentState is not StartGameState)
                return;

            Score++;

            OnScoreChanged?.Invoke(Score);
        }
    }
}
