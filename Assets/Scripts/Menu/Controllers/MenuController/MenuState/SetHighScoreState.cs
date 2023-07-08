using Cysharp.Threading.Tasks;
using Entry.Models;
using Entry.Services;
using Zenject;

namespace Menu.Controllers
{
    public class SetHighScoreState : MenuState
    {
        [Inject]
        private GameStateService gameStateService;

        [Inject]
        private HighScoresService highScoresService;

        [Inject]
        private MenuController menuController;

        public override async UniTask OnEnter()
        {
            Score score = gameStateService.GetLastScore();

            if (score != null && highScoresService.IsScoreHighEnough(score.value))
            {
                var context = new SetHighScorePanelContext(
                    $"Congratulations! You got a score of {score.value}, and made it to high scores!\nEnter your initials:",
                    score.name);

                menuController.OpenPanel(context);

                score.name = await context.WaitForInput();

                highScoresService.AddHighScore(score);
            }

            StateMachine.Transition<MainMenuState>();
        }
    }
}
