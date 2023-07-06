using System;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Controllers
{
    public class GameInputController : IController, IInitializable, IDisposable
    {
        [Inject]
        private InputControls gameControls;

        public event Action OnBirdJump;
        public event Action OnInteract;

        public void Initialize()
        {
            gameControls.Game.Jump.performed += Jump;
            gameControls.Game.Interact.performed += Interact;
        }

        private void Jump(InputAction.CallbackContext _)
        {
            OnBirdJump?.Invoke();
        }

        private void Interact(InputAction.CallbackContext _)
        {
            OnInteract?.Invoke();
        }

        public void BirdEnable()
        {
            gameControls.Game.Jump.Enable();
            gameControls.Game.Move.Enable();
        }

        public void BirdDisable()
        {
            gameControls.Game.Jump.Disable();
            gameControls.Game.Move.Disable();
        }

        public float BirdMoveValue() => gameControls.Game.Move.ReadValue<float>();

        public void InteractionEnable()
        {
            gameControls.Game.Interact.Enable();
        }

        public void InteractionDisable()
        {
            gameControls.Game.Interact.Disable();
        }

        public void Dispose()
        {
            gameControls.Game.Jump.performed -= Jump;
            gameControls.Game.Interact.performed -= Interact;
        }
    }
}
