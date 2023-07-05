using UnityEngine;

namespace Fp.Game.Views
{
    public class GameView : MonoBehaviour
    {
        [SerializeField]
        private Bird bird;

        [SerializeField]
        private WallsView wallsView;

        [SerializeField]
        private ObstaclesView obstaclesView;
    }
}
