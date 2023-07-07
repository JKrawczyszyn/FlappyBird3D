using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Game.Controllers
{
    public class CountdownState : GameplayState
    {
        public event Action<float> OnStart;

        [Inject]
        private GameConfig gameConfig;

        public override async UniTask OnEnter()
        {
            await Countdown();

            StateMachine.Transition<StartGameState>();
        }

        private async UniTask Countdown()
        {
            var time = gameConfig.gameplayConfig.countdownTime;

            OnStart?.Invoke(time);

            await UniTask.Delay(TimeSpan.FromSeconds(time));
        }
    }
}
