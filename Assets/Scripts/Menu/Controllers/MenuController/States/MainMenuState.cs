namespace Menu.Controllers
{
    public class MainMenuState : MenuState
    {
        public override void Perform()
        {
            MenuController.RemoveButtons();
            MenuController.AddButton("Start Game", MenuController.StartGame);
            MenuController.AddButton("High Scores", MenuController.HighScores);
        }
    }
}
