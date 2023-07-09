using Entry.Controllers;
using UnityEngine;
using Zenject;

namespace Entry
{
    public class EntryPoint : MonoBehaviour
    {
        [Inject]
        private FlowController flowController;

        [Inject]
        private Config config;

        [Inject]
        public void Construct()
        {
            Debug.Log("Start EntryPoint.");

            SetParameters();

            flowController.Start();
        }

        private void SetParameters()
        {
            Application.targetFrameRate = config.frameRate;
            Physics.gravity = Vector3.down * config.gameplayConfig.gravity;
            Time.timeScale = config.gameplayConfig.timeScale;
        }
    }
}
