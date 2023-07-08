using System.Collections.Generic;
using Menu.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Views
{
    public class MenuPanel : MonoBehaviour, IPanel
    {
        [SerializeField]
        private Button buttonPrefab;

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private Transform container;

        private readonly List<Button> buttons = new();

        public GameObject GameObject => gameObject;

        public void Initialize(MainMenuPanelContext context)
        {
            titleText.text = context.Title;

            foreach (var label in context.ButtonLabels)
                AddButton(label);
            
            foreach (var button in buttons)
                button.onClick.AddListener(() => { context.ButtonPressed = buttons.IndexOf(button); });
        }

        private void AddButton(string label)
        {
            var button = Instantiate(buttonPrefab, container);
            button.GetComponentInChildren<TextMeshProUGUI>().text = label;

            buttons.Add(button);
        }
    }
}
