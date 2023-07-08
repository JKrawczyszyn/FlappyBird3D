using Entry.Models;
using Menu.Controllers;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using Zenject;

namespace Menu.Views
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private MenuController menuController;

        [Inject]
        private PanelFactory panelFactory;

        private InputSystemUIInputModule inputModule;

        private Asset[] assets;

        private IPanel currentPanel;

        [Inject]
        private void Construct()
        {
            menuController.OnOpenPanel += OpenPanel;
            menuController.OnBack += Back;
            menuController.OnMove += Move;

            inputModule = FindObjectOfType<InputSystemUIInputModule>();
            inputModule.cancel.action.performed += menuController.Back;
            inputModule.move.action.performed += menuController.Move;
        }

        private void OpenPanel(IPanelContext context)
        {
            if (currentPanel != null)
                Destroy(currentPanel.GameObject);

            currentPanel = panelFactory.Create(context, container);
        }

        private void Back()
        {
            currentPanel.Back();
        }

        private void Move()
        {
            currentPanel.Move();
        }

        public void OnDestroy()
        {
            menuController.OnOpenPanel -= OpenPanel;
            menuController.OnBack -= Back;
            menuController.OnMove -= Move;

            inputModule.cancel.action.performed -= menuController.Back;
            inputModule.move.action.performed -= menuController.Move;
        }
    }
}
