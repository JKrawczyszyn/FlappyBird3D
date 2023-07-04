using Game.Controllers;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class BirdView : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbody;

        [Inject]
        private BirdController controller;

        private void Start()
        {
            rigidbody.mass = 0f;

            controller.OnSetMass += SetMass;
            controller.OnAddForce += AddForce;
            controller.OnSetVelocityX += SetVelocityX;
            controller.OnSetVelocityY += SetVelocityY;
            controller.OnClampVelocityX += ClampVelocityX;

            controller.Init();
        }

        private void SetMass(float mass)
        {
            rigidbody.mass = mass;
        }

        private void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            rigidbody.AddForce(force, mode);
        }

        private void SetVelocityX(float velocity)
        {
            rigidbody.velocity = new Vector3(velocity, rigidbody.velocity.y, 0f);
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
            controller.TriggerEnter(other);
        }

        private void OnDestroy()
        {
            controller.OnSetMass -= SetMass;
            controller.OnAddForce -= AddForce;
            controller.OnSetVelocityX -= SetVelocityX;
            controller.OnSetVelocityY -= SetVelocityY;
            controller.OnClampVelocityX -= ClampVelocityX;
        }
    }
}
