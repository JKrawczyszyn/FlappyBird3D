using Game.Controllers;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class CollisionResolver : MonoBehaviour
    {
        [Inject]
        private GameplayController gameplayController;

        [Inject]
        private CollectiblesView collectiblesView;

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
            if (collider.CompareTag(Constants.ObstacleTag))
                gameplayController.LostGame();
            else if (collider.CompareTag(Constants.CollectibleTag))
            {
                var id = collectiblesView.GetId(collider.gameObject.GetComponent<Collectible>());

                collectiblesController.Remove(id);

                scoreController.AddScore();
            }
        }
    }
}
