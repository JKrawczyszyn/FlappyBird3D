using System;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Controllers
{
    public class GameInputController : IController, IInitializable, IDisposable
    {
        [Inject]
        private InputControls inputActions;

        public event Action OnBirdJump;

        public void Initialize()
        {
            inputActions.Game.Jump.performed += Jump;
        }

        private void Jump(InputAction.CallbackContext _)
        {
            OnBirdJump?.Invoke();
        }

        public async UniTask WaitForInteraction()
        {
            inputActions.Game.Interact.Enable();

            await UniTask.WaitUntil(() => inputActions.Game.Interact.triggered);

            inputActions.Game.Interact.Disable();
        }

        public void BirdEnable()
        {
            inputActions.Game.Jump.Enable();
            inputActions.Game.Move.Enable();
        }

        public void BirdDisable()
        {
            inputActions.Game.Jump.Disable();
            inputActions.Game.Move.Disable();
        }

        public float BirdMoveValue() => inputActions.Game.Move.ReadValue<float>();

        public void Dispose()
        {
            inputActions.Game.Jump.performed -= Jump;
        }
    }
}
