using System;
using System.Collections.Generic;
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

        private string[] assetNames;

        private readonly List<Button> buttons = new();

        private void Start()
        {
            Initialize().Forget();
        }

        private async UniTaskVoid Initialize()
        {
            assetNames = assetsRepository.AssetNames(AssetTag.MenuButton);

            await assetsProvider.CacheReferences<Button>(assetNames);

            menuController.OnAddButton += AddButton;
            menuController.OnRemoveButtons += RemoveButtons;

            menuController.Initialize();
        }

        private void AddButton(string label, Action action)
        {
            var button = assetsProvider.Instantiate<Button>(assetNames.GetRandom(), Vector3.zero, container);
            button.GetComponentInChildren<TextMeshProUGUI>().text = label;
            button.onClick.AddListener(() => action());

            buttons.Add(button);

            if (buttons.Count == 1)
                EventSystem.current.SetSelectedGameObject(button.gameObject);
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
