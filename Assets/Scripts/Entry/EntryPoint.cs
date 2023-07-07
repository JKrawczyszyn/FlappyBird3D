using Entry.Controllers;
using UnityEngine;
using Zenject;

namespace Entry
{
    public class EntryPoint : MonoBehaviour
    {
        [Inject]
        private GameFlowController gameFlowController;

        private void Start()
        {
            Debug.Log("Start EntryPoint.");

            gameFlowController.Initialize();
        }
    }
}
