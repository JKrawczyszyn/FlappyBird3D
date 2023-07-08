using System;
using Utilities.FSM;
using Zenject;

namespace Menu.Controllers
{
    public class MenuController : IDisposable
    {
        public event Action<IPanelContext> OnOpenPanel;

        [Inject]
        private readonly StateMachine<MenuState> stateMachine;

        public void Initialize()
        {
            stateMachine.Transition<SetHighScoreState>();
        }

        public void Dispose()
        {
            stateMachine.Stop();
        }

        public void OpenPanel(IPanelContext context)
        {
            OnOpenPanel?.Invoke(context);
        }
    }
}
