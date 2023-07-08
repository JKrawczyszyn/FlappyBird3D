using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Game.Controllers
{
    public class StartGameState : GameplayState
    {
        public event Action OnStart;

        [Inject]
        private BirdController birdController;

        [Inject]
        private SpeedController speedController;

        [Inject]
        private Config config;

        public override async UniTask OnEnter()
        {
            speedController.SetSpeed(config.gameplayConfig.startSpeed);

            birdController.EnableInteraction();

            OnStart?.Invoke();

            await UniTask.Yield();
        }
    }
}
