using System;
using Entry;
using UnityEngine;
using Zenject;

namespace Game.Controllers
{
    public class BirdController : IController, ITickable, IDisposable
    {
        public event Action OnStartFly;
        public event Action OnKill;
        public event Action<float> OnSetMass;
        public event Action<float> OnSetVelocityY;
        public event Action<float, bool> OnAddForce;
        public event Action<float, float> OnClampVelocityX;

        [Inject]
        private Config config;

        [Inject]
        private GameInputController gameInputController;

        public void Initialize()
        {
            gameInputController.OnBirdJump += Jump;

            OnSetMass?.Invoke(config.birdConfig.mass);
        }

        public void Start()
        {
            gameInputController.BirdEnable();

            OnStartFly?.Invoke();
        }

        public void Kill()
        {
            gameInputController.BirdDisable();

            OnKill?.Invoke();
        }

        private void Jump()
        {
            OnSetVelocityY?.Invoke(0f);
            OnAddForce?.Invoke(config.birdConfig.jumpForce, true);
        }

        public void Tick()
        {
            float value = gameInputController.BirdMoveValue();
            if (value != 0f)
                Move(value * Time.deltaTime);
        }

        private void Move(float value)
        {
            OnAddForce?.Invoke(value, false);
            OnClampVelocityX?.Invoke(-config.birdConfig.maxSpeed, config.birdConfig.maxSpeed);
        }

        public void Dispose()
        {
            gameInputController.OnBirdJump -= Jump;
            gameInputController.BirdDisable();
        }
    }
}
