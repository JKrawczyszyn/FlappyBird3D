using System;
using System.Globalization;
using Entry.Models;
using TMPro;
using UnityEngine;

namespace Menu.Views
{
    public class HighScoresEntry : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI placeText;

        [SerializeField]
        private TextMeshProUGUI nameText;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        [SerializeField]
        private TextMeshProUGUI dateTimeText;

        public void Initialize(int place, Score score)
        {
            placeText.text = $"{place}.";
            nameText.text = score.name;
            scoreText.text = score.value.ToString();
            dateTimeText.text = new DateTime(score.timeTicks).ToString(CultureInfo.CurrentCulture);
        }
    }
}
