using System;
using Cysharp.Threading.Tasks;
using Entry.Controllers;
using Entry.Models;
using Zenject;

namespace Game.Controllers
{
    public class LostGameState : GameplayState
    {
        public event Action<Score> OnStart;

        [Inject]
        private SpeedController speedController;

        [Inject]
        private GameInputController gameInputController;

        [Inject]
        private FlowController flowController;

        [Inject]
        private GameConfig gameConfig;

        private Score score;

        public override async UniTask OnEnter()
        {
            score = new Score((int)Data, DateTime.Now, "Player");

            speedController.SetSpeed(0f);

            gameInputController.BirdDisable();

            OnStart?.Invoke(score);

            await UniTask.Delay(TimeSpan.FromSeconds(gameConfig.gameplayConfig.endGameInteractionDelay));

            gameInputController.OnInteract += Interact;
            gameInputController.InteractionEnable();
        }

        private void Interact()
        {
            gameInputController.OnInteract -= Interact;
            gameInputController.InteractionDisable();

            flowController.LoadMenu(score);
        }
    }
}
