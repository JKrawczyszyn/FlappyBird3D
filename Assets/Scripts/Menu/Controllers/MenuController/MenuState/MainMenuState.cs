using System;
using Cysharp.Threading.Tasks;
using Entry.Controllers;
using Zenject;

namespace Menu.Controllers
{
    public class MainMenuState : MenuState
    {
        [Inject]
        private MenuController menuController;

        [Inject]
        private FlowController flowController;

        public override async UniTask OnEnter()
        {
            menuController.SetButtons("Start Game", "High Scores");

            var result = await menuController.WaitForButtonResult();

            switch (result)
            {
                case 0:
                    LoadGame();
                    break;
                case 1:
                    LoadHighScore();
                    break;
                default:
                    throw new ArgumentException($"Button index '{result}' not supported.");
            }
        }

        private void LoadGame()
        {
            flowController.LoadGame();

            StateMachine.Stop();
        }

        private void LoadHighScore()
        {
            StateMachine.RequestTransition(typeof(HighScoresState));
        }
    }
}
