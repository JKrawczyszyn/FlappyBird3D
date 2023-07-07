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
        private CountdownState countdownState;

        [Inject]
        public void Construct()
        {
            countdownState.OnStart += CountdownStart;
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
            countdownState.OnStart -= CountdownStart;
        }
    }
}
