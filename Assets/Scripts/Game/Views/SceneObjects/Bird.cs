using Game.Controllers;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class Bird : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbody;

        [Inject]
        private BirdController controller;

        private void Start()
        {
            controller.Initialize(rigidbody);
        }

        public void OnCollisionStay(Collision other)
        {
            controller.BirdCollision(other);
        }
    }
}
