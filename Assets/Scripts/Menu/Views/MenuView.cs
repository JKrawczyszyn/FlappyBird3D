using Entry.Models;
using Menu.Controllers;
using UnityEngine;
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

        private Asset[] assets;

        private IPanel currentPanel;

        [Inject]
        private void Construct()
        {
            menuController.OnOpenPanel += OpenPanel;
        }

        private void OpenPanel(IPanelContext context)
        {
            if (currentPanel != null)
                Destroy(currentPanel.GameObject);

            currentPanel = panelFactory.Create(context, container);
        }

        public void OnDestroy()
        {
            menuController.OnOpenPanel -= OpenPanel;
        }
    }
}
