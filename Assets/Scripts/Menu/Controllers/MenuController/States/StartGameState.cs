using Entry.Controllers;
using Zenject;

namespace Menu.Controllers
{
    public class StartGameState : MenuState
    {
        [Inject]
        private GameFlowController gameFlowController;

        public override void Perform()
        {
            gameFlowController.StartGame();
        }
    }
}
