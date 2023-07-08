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

        private void Start()
        {
            Debug.Log("Start EntryPoint.");

            SetParameters();

            flowController.Initialize();
        }

        private void SetParameters()
        {
            Application.targetFrameRate = config.frameRate;
            Physics.gravity = Vector3.down * config.gravity;
        }
    }
}
