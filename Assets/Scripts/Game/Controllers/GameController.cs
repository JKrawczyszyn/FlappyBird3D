using System;
using Zenject;

namespace Fp.Game.Controllers
{
    public class GameController
    {
        public event Action OnGameStarted;

        [Inject]
        private BirdController birdController;

        [Inject]
        private SpeedController speedController;

        [Inject]
        private GameConfig gameConfig;

        public void StartGame()
        {
            speedController.SetSpeed(gameConfig.obstaclesConfig.startSpeed);

            birdController.Start();

            OnGameStarted?.Invoke();
        }
    }
}
