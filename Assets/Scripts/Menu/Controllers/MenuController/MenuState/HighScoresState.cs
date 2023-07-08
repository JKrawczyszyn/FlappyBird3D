using Cysharp.Threading.Tasks;
using Entry.Services;
using Zenject;

namespace Menu.Controllers
{
    public class HighScoresState : MenuState
    {
        [Inject]
        private HighScoresService highScoresService;

        [Inject]
        private MenuController menuController;

        public override async UniTask OnEnter()
        {
            var highScores = highScoresService.GetHighScores();

            var context = new HighScoresPanelContext("High Scores", highScores, "Return to Main Menu");

            menuController.OpenPanel(context);

            await context.WaitForButton();

            StateMachine.Transition<MainMenuState>();
        }
    }
}
