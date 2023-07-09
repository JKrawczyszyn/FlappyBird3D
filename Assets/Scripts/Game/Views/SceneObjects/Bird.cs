using Game.Controllers;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class Bird : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbody;

        [SerializeField]
        private Animator animator;

        [Inject]
        private CollisionResolver collisionResolver;

        [Inject]
        private BirdController controller;

        private static readonly int start = Animator.StringToHash("start");

        [Inject]
        private void Construct()
        {
            controller.OnStartFly += StartFly;
            controller.OnKill += Kill;
            controller.OnSetMass += SetMass;
            controller.OnSetVelocityY += SetVelocityY;
            controller.OnAddForce += AddForce;
            controller.OnClampVelocityX += ClampVelocityX;

            rigidbody.isKinematic = true;

            controller.Initialize();
        }

        private void StartFly()
        {
            rigidbody.isKinematic = false;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            animator.SetTrigger(start);
        }

        private void Kill()
        {
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        private void SetMass(float value)
        {
            rigidbody.mass = value;
        }

        private void SetVelocityY(float value)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, value, 0f);
        }

        private void AddForce(float value, bool up)
        {
            var direction = up ? Vector3.up : Vector3.right;
            rigidbody.AddForce(direction * value, ForceMode.Impulse);
        }

        private void ClampVelocityX(float min, float max)
        {
            var clamped = Mathf.Clamp(rigidbody.velocity.x, min, max);
            rigidbody.velocity = new Vector3(clamped, rigidbody.velocity.y, 0f);
        }

        private void OnTriggerStay(Collider other)
        {
            collisionResolver.BirdCollision(other);
        }

        private void OnCollisionEnter(Collision other)
        {
            collisionResolver.BirdCollision(other);
        }

        private void OnDestroy()
        {
            controller.OnStartFly -= StartFly;
            controller.OnKill -= Kill;
            controller.OnSetMass -= SetMass;
            controller.OnSetVelocityY -= SetVelocityY;
            controller.OnAddForce -= AddForce;
            controller.OnClampVelocityX -= ClampVelocityX;
        }
    }
}
