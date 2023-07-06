namespace Menu.Controllers
{
    public class HighScoresMenuState : MenuState
    {
        public override void Perform()
        {
            MenuController.RemoveButtons();
            MenuController.AddButton("High Scores List", MenuController.Back);
        }
    }
}
