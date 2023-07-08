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
            var context = new MainMenuPanelContext("Flappy Bird 3D", "Start Game", "High Scores");

            menuController.OpenPanel(context);

            int result = await context.WaitForButton();

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
            StateMachine.Transition<HighScoresState>();
        }
    }
}
