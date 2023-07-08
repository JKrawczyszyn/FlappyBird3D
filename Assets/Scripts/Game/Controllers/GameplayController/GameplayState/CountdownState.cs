using System;
using Cysharp.Threading.Tasks;
using Entry;
using Zenject;

namespace Game.Controllers
{
    public class CountdownState : GameplayState
    {
        public event Action<float> OnStart;

        [Inject]
        private Config config;

        public override async UniTask OnEnter()
        {
            await Countdown();

            StateMachine.Transition<StartGameState>();
        }

        private async UniTask Countdown()
        {
            float time = config.gameplayConfig.countdownTime;

            OnStart?.Invoke(time);

            await UniTask.Delay(TimeSpan.FromSeconds(time));
        }
    }
}
