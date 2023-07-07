using System;
using Cysharp.Threading.Tasks;
using Utilities.FSM;
using Zenject;

namespace Menu.Controllers
{
    public class MenuController : IDisposable
    {
        public event Action<string, Action> OnAddButton;
        public event Func<UniTask<int>> OnWaitForButtonResult;
        public event Action OnRemoveButtons;

        [Inject]
        private readonly StateMachine<MenuState> stateMachine;

        public void Initialize()
        {
            stateMachine.Transition<MainMenuState>();
        }

        public void SetButtons(params string[] labels)
        {
            OnRemoveButtons?.Invoke();

            foreach (var label in labels)
                OnAddButton?.Invoke(label, () => { });
        }

        public async UniTask<int> WaitForButtonResult()
        {
            if (OnWaitForButtonResult != null)
                return await OnWaitForButtonResult();

            return -1;
        }

        public void Dispose()
        {
            stateMachine.Stop();
        }
    }
}
