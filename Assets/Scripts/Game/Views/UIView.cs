using Cysharp.Threading.Tasks;
using Game.Controllers;
using TMPro;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class UIView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;

        [SerializeField]
        private TextMeshProUGUI countdownText;

        [Inject]
        private GameController gameController;

        [Inject]
        public void Construct()
        {
            gameController.OnCountdownStart += CountdownStart;
        }

        private void CountdownStart(float time)
        {
            Countdown(time).Forget();
        }

        private async UniTask Countdown(float time)
        {
            countdownText.gameObject.SetActive(true);

            await countdownText.AnimateCount(time, 0f, time);

            countdownText.gameObject.SetActive(false);
        }

        public void OnDestroy()
        {
            gameController.OnCountdownStart -= CountdownStart;
        }
    }
}
