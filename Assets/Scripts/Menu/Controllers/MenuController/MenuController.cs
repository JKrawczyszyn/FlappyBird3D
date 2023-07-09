using System;
using UnityEngine.InputSystem;
using Utilities.States;
using Zenject;

namespace Menu.Controllers
{
    public class MenuController : IDisposable
    {
        public event Action<IPanelContext> OnOpenPanel;
        public event Action OnBack;
        public event Action OnMove;

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

        public void Back(InputAction.CallbackContext _)
        {
            OnBack?.Invoke();
        }

        public void Move(InputAction.CallbackContext _)
        {
            OnMove?.Invoke();
        }
    }
}
