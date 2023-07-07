using System;
using Cysharp.Threading.Tasks;
using Entry.Controllers;
using Zenject;

namespace Game.Controllers
{
    public class LostGameState : GameplayState
    {
        public event Action<int> OnStart;

        [Inject]
        private SpeedController speedController;

        [Inject]
        private GameInputController gameInputController;

        [Inject]
        private FlowController flowController;

        [Inject]
        private GameConfig gameConfig;

        public override async UniTask OnEnter()
        {
            speedController.SetSpeed(0f);

            gameInputController.BirdDisable();

            OnStart?.Invoke((int)Data);

            await UniTask.Delay(TimeSpan.FromSeconds(gameConfig.gameplayConfig.endGameInteractionDelay));

            gameInputController.OnInteract += Interact;
            gameInputController.InteractionEnable();
        }

        private void Interact()
        {
            gameInputController.OnInteract -= Interact;
            gameInputController.InteractionDisable();

            flowController.LoadMenu();
        }
    }
}
