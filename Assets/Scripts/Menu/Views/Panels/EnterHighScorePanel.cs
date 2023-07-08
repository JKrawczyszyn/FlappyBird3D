using System.Collections.Generic;
using System.Linq;
using Menu.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Views
{
    public class EnterHighScorePanel : MonoBehaviour, IPanel
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
    }
}
