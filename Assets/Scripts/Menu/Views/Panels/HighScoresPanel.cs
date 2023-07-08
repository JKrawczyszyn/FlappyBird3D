using Menu.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu.Views
{
    public class HighScoresPanel : MonoBehaviour, IPanel
    {
        [SerializeField]
        private TextMeshProUGUI textPrefab;

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private Transform container;

        [SerializeField]
        private Button button;

        public GameObject GameObject => gameObject;

        public void Initialize(HighScoresPanelContext context)
        {
            titleText.text = context.Title;

            var i = 1;
            foreach (var score in context.HighScores)
                AddText($"{i++}. {score}");

            button.GetComponentInChildren<TextMeshProUGUI>().text = context.ButtonLabel;

            button.onClick.AddListener(() =>
                                       {
                                           button.interactable = false;
                                           context.ButtonPressed = true;
                                       });
        }

        private void AddText(string label)
        {
            var instance = Instantiate(textPrefab, container);
            instance.text = label;
        }

        public void Back()
        {
            button.onClick.Invoke();
        }

        public void Move()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }
}
