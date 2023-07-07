using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Menu.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;
using Zenject;

namespace Menu.Views
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsProvider assetsProvider;

        [Inject]
        private MenuController menuController;

        private Asset[] assets;

        private readonly List<Button> buttons = new();

        [Inject]
        private async UniTaskVoid Construct()
        {
            assets = assetsRepository.AssetsForScene(SceneName.Menu);

            await assetsProvider.CacheReferences<Button>(assets.Select(a => a.name));

            menuController.OnAddButton += AddButton;
            menuController.OnWaitForButtonResult += WaitForButtonResult;
            menuController.OnRemoveButtons += RemoveButtons;

            menuController.Initialize();
        }

        private void AddButton(string label, Action action)
        {
            var buttonName = assets.Where(a => a.tag == AssetTag.MenuButton).GetRandom().name;

            var button = assetsProvider.Instantiate<Button>(buttonName, Vector3.zero, container);
            button.GetComponentInChildren<TextMeshProUGUI>().text = label;

            buttons.Add(button);

            if (buttons.Count == 1)
                EventSystem.current.SetSelectedGameObject(button.gameObject);
        }

        private async UniTask<int> WaitForButtonResult()
        {
            var tasks = buttons.Select(button => button.OnClickAsync());

            return await UniTask.WhenAny(tasks);
        }

        private void RemoveButtons()
        {
            foreach (var button in buttons)
                Destroy(button.gameObject);

            buttons.Clear();
        }

        public void OnDestroy()
        {
            menuController.OnAddButton -= AddButton;
            menuController.OnRemoveButtons -= RemoveButtons;
        }
    }
}
