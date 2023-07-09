using Cysharp.Threading.Tasks;
using Entry;
using Zenject;

namespace Game.Controllers
{
    public class StartGameState : GameplayState
    {
        [Inject]
        private BirdController birdController;

        [Inject]
        private SpeedController speedController;

        [Inject]
        private Config config;

        public override async UniTask OnEnter()
        {
            speedController.SetSpeed(config.gameplayConfig.startSpeed);

            birdController.Start();

            await UniTask.Yield();
        }
    }
}
