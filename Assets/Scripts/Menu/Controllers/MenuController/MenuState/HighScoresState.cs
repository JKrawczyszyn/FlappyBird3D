using Cysharp.Threading.Tasks;
using Zenject;

namespace Menu.Controllers
{
    public class HighScoresState : MenuState
    {
        [Inject]
        private MenuController menuController;

        public override async UniTask OnEnter()
        {
            menuController.SetButtons("High Scores");

            await menuController.WaitForButtonResult();

            StateMachine.Transition<MainMenuState>();
        }
    }
}
