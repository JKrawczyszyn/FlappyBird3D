using System;
using Zenject;

namespace Game.Controllers
{
    public class ScoreController : IController, IDisposable
    {
        public event Action<int> OnScoreChanged;

        [Inject]
        private ObstaclesController obstaclesController;

        [Inject]
        private GameplayController gameplayController;

        public int Score { get; private set; }

        [Inject]
        private void Construct()
        {
            obstaclesController.OnPassed += Passed;

            Score = 0;
        }

        private void Passed(ObstacleModel _)
        {
            if (gameplayController.CurrentState is not StartGameState)
                return;

            Score++;

            OnScoreChanged?.Invoke(Score);
        }

        public void Dispose()
        {
            obstaclesController.OnPassed -= Passed;
        }
    }
}
