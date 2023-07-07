using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Game.Controllers
{
    public class PlayState : GameplayState
    {
        public event Action OnStart;

        [Inject]
        private BirdController birdController;

        [Inject]
        private SpeedController speedController;

        [Inject]
        private GameConfig gameConfig;

        public override async UniTask OnEnter()
        {
            speedController.SetSpeed(gameConfig.obstaclesConfig.startSpeed);

            birdController.EnableInteraction();

            OnStart?.Invoke();

            await UniTask.Yield();
        }
    }
}
