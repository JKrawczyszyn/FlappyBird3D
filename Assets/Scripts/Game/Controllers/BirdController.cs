using System;
using TMPro;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Controllers
{
    public class BirdController : IController, ITickable, IDisposable
    {
        public event Action<bool> OnSetMovable;
        public event Action<float> OnSetMass;
        public event Action<float> OnSetVelocityY;
        public event Action<float, bool> OnAddForce;
        public event Action<float, float> OnClampVelocityX;

        [Inject]
        private GameConfig gameConfig;

        [Inject]
        private GameInputController gameInputController;

        [Inject]
        private GameplayController gameplayController;

        public void Initialize()
        {
            gameInputController.OnBirdJump += Jump;

            OnSetMovable?.Invoke(false);
            OnSetMass?.Invoke(gameConfig.birdConfig.mass);
        }

        public void EnableInteraction()
        {
            gameInputController.BirdEnable();

            OnSetMovable?.Invoke(true);
        }

        public void DisableInteraction()
        {
            gameInputController.BirdDisable();

            OnSetMovable?.Invoke(false);
        }

        private void Jump()
        {
            OnSetVelocityY?.Invoke(0f);
            OnAddForce?.Invoke(gameConfig.birdConfig.jumpForce, true);
        }

        public void Tick()
        {
            var value = gameInputController.BirdMoveValue();
            if (value != 0f)
                Move(value * Time.deltaTime);
        }

        private void Move(float value)
        {
            OnAddForce?.Invoke(value, false);
            OnClampVelocityX?.Invoke(-gameConfig.birdConfig.maxSpeed, gameConfig.birdConfig.maxSpeed);
        }

        public void Collision(Collision other)
        {
            if (other.collider.CompareTag(Constants.ObstacleTag))
                gameplayController.LostGame();
        }

        public void Dispose()
        {
            gameInputController.OnBirdJump -= Jump;
            gameInputController.BirdDisable();
        }
    }
}
