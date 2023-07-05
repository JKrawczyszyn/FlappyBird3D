using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Fp.Game.Controllers
{
    public class BirdController : ITickable, IDisposable
    {
        public event Action OnStartGravity;
        public event Action<Vector3, ForceMode> OnAddForce;
        public event Action<float> OnSetVelocityY;
        public event Action<float> OnClampVelocityX;

        [Inject]
        private GameConfig gameConfig;

        [Inject]
        private GameControls gameControls;

        public float Mass => gameConfig.birdConfig.mass;

        [Inject]
        public void Construct()
        {
            gameControls.Map.Jump.performed += Jump;
        }

        public void Start()
        {
            gameControls.Map.Jump.Enable();
            gameControls.Map.Move.Enable();

            OnStartGravity?.Invoke();
        }

        private void Jump(InputAction.CallbackContext _)
        {
            OnSetVelocityY?.Invoke(0f);
            OnAddForce?.Invoke(Vector3.up * gameConfig.birdConfig.jumpForce, ForceMode.Impulse);
        }

        public void Tick()
        {
            var moveAction = gameControls.Map.Move;
            if (moveAction.IsPressed())
                Move(moveAction.ReadValue<float>() * Time.deltaTime);
        }

        private void Move(float value)
        {
            // Debug.Log(">>> Move: " + value);

            OnAddForce?.Invoke(Vector3.right * value, ForceMode.Impulse);
            OnClampVelocityX?.Invoke(gameConfig.birdConfig.maxSpeed);
        }

        public void BirdCollision(Collision other)
        {
            Debug.Log(">>> BirdCollision: " + other.collider.tag);
        }

        public void Dispose()
        {
            gameControls.Map.Jump.performed -= Jump;
            gameControls.Map.Disable();
        }
    }
}
