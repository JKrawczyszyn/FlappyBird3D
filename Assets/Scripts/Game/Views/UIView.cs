using Cysharp.Threading.Tasks;
using Entry.Models;
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

        [SerializeField]
        private TextMeshProUGUI gameOverText;

        [Inject]
        private ScoreController scoreController;

        [Inject]
        private CountdownState countdownState;

        [Inject]
        private LostGameState lostGameState;

        [Inject]
        public void Construct()
        {
            scoreController.OnScoreChanged += ScoreChanged;
            countdownState.OnStart += CountdownStart;
            lostGameState.OnStart += LostGameStart;

            ScoreChanged(scoreController.Score);

            scoreText.gameObject.SetActive(true);
            countdownText.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
        }

        private void ScoreChanged(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        private void CountdownStart(float time)
        {
            Countdown(time).Forget();
        }

        private async UniTask Countdown(float time)
        {
            countdownText.gameObject.SetActive(true);

            await countdownText.AnimateCount(time, 0f, time, gameObject.GetCancellationTokenOnDestroy());

            countdownText.gameObject.SetActive(false);
        }

        private void LostGameStart(Score score)
        {
            gameOverText.text = $"Game Over\nScore: {score.value}";

            gameOverText.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(false);
        }

        public void OnDestroy()
        {
            scoreController.OnScoreChanged -= ScoreChanged;
            countdownState.OnStart -= CountdownStart;
            lostGameState.OnStart -= LostGameStart;
        }
    }
}
