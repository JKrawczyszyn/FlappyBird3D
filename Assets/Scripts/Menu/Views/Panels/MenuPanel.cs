using System.Collections.Generic;
using Menu.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
                button.onClick.AddListener(() =>
                                           {
                                               button.interactable = false;
                                               context.ButtonPressed = buttons.IndexOf(button);
                                           });
        }

        private void AddButton(string label)
        {
            var instance = Instantiate(buttonPrefab, container);
            instance.GetComponentInChildren<TextMeshProUGUI>().text = label;

            buttons.Add(instance);
        }

        public void Back()
        {
        }

        public void Move()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        }
    }
}
