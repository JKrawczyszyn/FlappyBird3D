using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public static class AnimationsExtensions
    {
        public static async UniTask AnimateAlpha(this CanvasGroup image, float start, float end, float timeSeconds)
        {
            var startTime = Time.time;
            var endTime = startTime + timeSeconds;

            while (Time.time < endTime)
            {
                var fraction = (Time.time - startTime) / timeSeconds;
                image.alpha = Mathf.Lerp(start, end, fraction);

                await UniTask.NextFrame();
            }

            image.alpha = end;
        }

        public static async UniTask AnimateCount(this TextMeshProUGUI text, float start, float end, float timeSeconds)
        {
            var startTime = Time.time;
            var endTime = startTime + timeSeconds;

            while (Time.time < endTime)
            {
                var fraction = (Time.time - startTime) / timeSeconds;
                text.text = Mathf.Ceil(Mathf.Lerp(start, end, fraction)).ToString("0");

                await UniTask.NextFrame();
            }

            text.text = Mathf.Ceil(end).ToString("0");
        }
    }
}
