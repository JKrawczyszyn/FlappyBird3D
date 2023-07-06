using UnityEngine;
using Zenject;

namespace Game
{
    public class GameEntryPoint : MonoBehaviour
    {
        [Inject]
        private GameConfig gameConfig;

        private void Start()
        {
            Debug.Log("Start GameEntryPoint.");

            SetConfigs();
        }

        private void SetConfigs()
        {
            Application.targetFrameRate = gameConfig.frameRate;
            Physics.gravity = Vector3.down * gameConfig.gravity;
        }
    }
}
