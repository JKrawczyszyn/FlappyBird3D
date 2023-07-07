using System;
using Entry.Controllers;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Controllers
{
    public class BirdController : IController, ITickable, IDisposable
    {
        [Inject]
        private GameConfig gameConfig;

        [Inject]
        private GameInputController gameInputController;

        [Inject]
        private GameFlowController gameFlowController;

        private Rigidbody rigidbody;

        public void Initialize(Rigidbody rigidbody)
        {
            this.rigidbody = rigidbody;

            rigidbody.isKinematic = true;
            rigidbody.mass = gameConfig.birdConfig.mass;

            gameInputController.OnBirdJump += Jump;
        }

        public void EnableInteraction()
        {
            gameInputController.BirdEnable();

            rigidbody.isKinematic = false;
        }

        private void Jump()
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, 0f);
            rigidbody.AddForce(Vector3.up * gameConfig.birdConfig.jumpForce, ForceMode.Impulse);
        }

        public void Tick()
        {
            var value = gameInputController.BirdMoveValue();
            if (value != 0f)
                Move(value * Time.deltaTime);
        }

        private void Move(float value)
        {
            rigidbody.AddForce(Vector3.right * value, ForceMode.Impulse);
            rigidbody.velocity
                = new Vector3(
                    Mathf.Clamp(rigidbody.velocity.x, -gameConfig.birdConfig.maxSpeed, gameConfig.birdConfig.maxSpeed),
                    rigidbody.velocity.y, 0f);
        }

        public void BirdCollision(Collision other)
        {
            if (other.collider.CompareTag(Constants.ObstacleTag))
                gameFlowController.LostGame(10);
        }

        public void Dispose()
        {
            gameInputController.OnBirdJump -= Jump;
            gameInputController.BirdDisable();
        }
    }
}
