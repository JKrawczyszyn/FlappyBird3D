using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Game.Controllers
{
    public class GameController : IController
    {
        public event Action<float> OnCountdownStart;
        public event Action OnGameStarted;

        [Inject]
        private BirdController birdController;

        [Inject]
        private SpeedController speedController;

        [Inject]
        private GameConfig gameConfig;

        public async UniTask StartGame()
        {
            await Countdown();

            speedController.SetSpeed(gameConfig.obstaclesConfig.startSpeed);

            birdController.EnableInteraction();

            OnGameStarted?.Invoke();
        }

        private async UniTask Countdown()
        {
            var time = gameConfig.gameplayConfig.countdownTime;

            OnCountdownStart?.Invoke(time);

            await UniTask.Delay(TimeSpan.FromSeconds(time));
        }
    }
}
