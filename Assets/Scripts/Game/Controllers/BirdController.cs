using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Controllers
{
    public class BirdController : ITickable, IDisposable
    {
        public event Action<float> OnSetMass;
        public event Action<Vector3, ForceMode> OnAddForce;
        public event Action<float> OnSetVelocityX;
        public event Action<float> OnSetVelocityY;
        public event Action<float> OnClampVelocityX;

        [Inject]
        private GameConfig config;

        [Inject]
        private GameControls controls;

        public void Init()
        {
            OnSetMass?.Invoke(config.birdMass);

            controls.Map.Jump.performed += Jump;
            controls.Map.Enable();
        }

        private void Jump(InputAction.CallbackContext _)
        {
            OnSetVelocityY?.Invoke(0f);
            OnAddForce?.Invoke(Vector3.up * config.birdJumpForce, ForceMode.Impulse);
        }

        public void Tick()
        {
            var moveAction = controls.Map.Move;
            if (moveAction.IsPressed())
                Move(moveAction.ReadValue<float>() * Time.deltaTime);
        }

        private void Move(float value)
        {
            // Debug.Log(">>> Move: " + value);

            OnAddForce?.Invoke(Vector3.right * value, ForceMode.Impulse);
            OnClampVelocityX?.Invoke(config.maxBirdSpeed);
        }

        public void TriggerEnter(Collision other)
        {
            Debug.Log(">>> TriggerEnter: " + other.collider.tag);

            // OnSetVelocityX?.Invoke(0f);
        }

        public void Dispose()
        {
            controls.Map.Jump.performed -= Jump;
            controls.Map.Disable();
        }
    }
}
