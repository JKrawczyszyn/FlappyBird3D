using Menu.Controllers;
using TMPro;
using UnityEngine;

namespace Menu.Views
{
    public class SetHighScorePanel : MonoBehaviour, IPanel
    {
        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TMP_InputField inputField;

        public GameObject GameObject => gameObject;

        public void Initialize(SetHighScorePanelContext context)
        {
            titleText.text = context.Title;
            inputField.text = context.Name;

            inputField.onEndEdit.AddListener(value =>
                                             {
                                                 context.Name = value;
                                                 context.NameEntered = true;
                                             });
        }

        public void Back()
        {
        }

        public void Move()
        {
        }
    }
}
