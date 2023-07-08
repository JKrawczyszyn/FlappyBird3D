using System.Text;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class DebugView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI debugText;

        [Inject]
        private GameConfig gameConfig;

        private float smoothedDeltaTime;

        private readonly string[] debugTexts = new string[2];
        private readonly StringBuilder stringBuilder = new();

        private void Awake()
        {
            if (!gameConfig.debugMode)
                Destroy(gameObject);
        }

        private void Update()
        {
            UpdateFps(0);
            UpdateGameObjects(1);

            UpdateText();
        }

        private void UpdateFps(int index)
        {
            smoothedDeltaTime += (Time.deltaTime - smoothedDeltaTime) * 0.1f;
            var fps = 1.0f / smoothedDeltaTime;

            debugTexts[index] = $"FPS: {Mathf.Ceil(fps)}";
        }

        private void UpdateGameObjects(int index)
        {
            var objects = FindObjectsOfType<GameObject>();

            debugTexts[index] = $"GameObjects: {objects.Length}";
        }

        private void UpdateText()
        {
            stringBuilder.Clear();
            stringBuilder.AppendJoin('\n', debugTexts);
            debugText.text = stringBuilder.ToString();
        }
    }
}
