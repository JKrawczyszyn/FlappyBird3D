using System;
using Utilities;
using Zenject;

namespace Menu.Controllers
{
    public class MenuController
    {
        [Inject]
        private readonly StateMachine<MenuState> stateMachine;

        public event Action<string, Action> OnAddButton;
        public event Action OnRemoveButtons;

        public void Initialize()
        {
            stateMachine.Push<MainMenuState>();
            stateMachine.Perform();
        }

        public void AddButton(string label, Action action)
        {
            OnAddButton?.Invoke(label, action);
        }

        public void RemoveButtons()
        {
            OnRemoveButtons?.Invoke();
        }

        public void StartGame()
        {
            stateMachine.Push<StartGameState>();
            stateMachine.Perform();
        }

        public void HighScores()
        {
            stateMachine.Push<HighScoresMenuState>();
            stateMachine.Perform();
        }

        public void Back()
        {
            stateMachine.Pop();
            stateMachine.Perform();
        }
    }
}
