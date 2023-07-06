using Menu.Controllers;
using UnityEngine;
using Zenject;

namespace Menu
{
    public class MenuEntryPoint : MonoBehaviour
    {
        [Inject]
        private MenuController menuController;

        private void Start()
        {
            Debug.Log("Start MenuEntryPoint.");
        }
    }
}
