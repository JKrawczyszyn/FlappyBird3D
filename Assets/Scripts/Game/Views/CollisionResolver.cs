using Game.Controllers;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class CollisionResolver : MonoBehaviour
    {
        [Inject]
        private CollectiblesView collectiblesView;

        [Inject]
        private GameplayController gameplayController;

        [Inject]
        private CollectiblesController collectiblesController;

        [Inject]
        private ScoreController scoreController;

        public void BirdCollision(Collision other)
        {
            BirdCollision(other.collider);
        }

        public void BirdCollision(Collider collider)
        {
            if (gameplayController.CurrentState is not StartGameState)
                return;

            if (collider.CompareTag(Constants.ObstacleTag))
                gameplayController.LostGame();
            else if (collider.CompareTag(Constants.CollectibleTag))
            {
                var collectible = collider.gameObject.GetComponent<Collectible>();

                Assert.IsNotNull(collectible, $"Collectible component is missing on '{collider.gameObject.name}'.");

                int id = collectiblesView.GetId(collectible);

                Assert.IsTrue(id > 0, $"Collectible id '{id}' is invalid.");

                collectiblesController.Remove(id);

                scoreController.AddScore();
            }
        }
    }
}
