using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Entry.Models;
using Entry.Services;
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
        private AssetsService assetsService;

        [Inject]
        private MenuController menuController;

        private Asset[] assets;

        private readonly List<Button> buttons = new();

        [Inject]
        private async UniTaskVoid Construct()
        {
            menuController.OnAddButton += AddButton;
            menuController.OnWaitForButtonResult += WaitForButtonResult;
            menuController.OnRemoveButtons += RemoveButtons;

            assets = assetsRepository.AssetsForScene(SceneName.Menu);

            await assetsService.CacheReferences<Button>(assets.Select(a => a.name));
        }

        private void AddButton(string label, Action action)
        {
            var buttonName = assets.Where(a => a.tag == AssetTag.MenuButton).GetRandom().name;

            var button = assetsService.Instantiate<Button>(buttonName, Vector3.zero, container);
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
