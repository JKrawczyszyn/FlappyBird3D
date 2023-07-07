using Entry.Controllers;
using UnityEngine;
using Zenject;

namespace Entry
{
    public class EntryPoint : MonoBehaviour
    {
        [Inject]
        private FlowController flowController;

        private void Start()
        {
            Debug.Log("Start EntryPoint.");

            flowController.Initialize();
        }
    }
}
