using Menu.Controllers;
using TMPro;
using UnityEngine;
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

            button.onClick.AddListener(() => context.ButtonPressed = true);
        }

        private void AddText(string label)
        {
            var button = Instantiate(textPrefab, container);
            button.GetComponentInChildren<TextMeshProUGUI>().text = label;
        }
    }
}
