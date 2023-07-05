using Fp.Game.Controllers;
using UnityEngine;
using Zenject;

namespace Fp.Game.Views
{
    public class Bird : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbody;

        [Inject]
        private BirdController controller;

        [Inject]
        public void Construct()
        {
            rigidbody.useGravity = false;

            SetMass(controller.Mass);

            controller.OnStartGravity += StartGravity;
            controller.OnAddForce += AddForce;
            controller.OnSetVelocityY += SetVelocityY;
            controller.OnClampVelocityX += ClampVelocityX;
        }

        private void StartGravity()
        {
            rigidbody.useGravity = true;
        }

        private void SetMass(float mass)
        {
            rigidbody.mass = mass;
        }

        private void AddForce(Vector3 force, ForceMode mode)
        {
            rigidbody.AddForce(force, mode);
        }

        private void SetVelocityY(float velocity)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, velocity, 0f);
        }

        private void ClampVelocityX(float velocity)
        {
            rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -velocity, velocity),
                                             rigidbody.velocity.y,
                                             0f);
        }

        public void OnCollisionStay(Collision other)
        {
            controller.BirdCollision(other);
        }

        private void OnDestroy()
        {
            controller.OnStartGravity -= StartGravity;
            controller.OnAddForce -= AddForce;
            controller.OnSetVelocityY -= SetVelocityY;
            controller.OnClampVelocityX -= ClampVelocityX;
        }
    }
}
