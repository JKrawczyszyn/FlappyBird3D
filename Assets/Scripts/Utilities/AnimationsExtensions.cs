using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Utilities
{
    // If more animations are required, it would be better to use some animation plugin, like DOTween.
    public static class AnimationsExtensions
    {
        public static async UniTask AnimateAlpha(this CanvasGroup image, float start, float end, float timeSeconds)
        {
            float startTime = Time.time;
            float endTime = startTime + timeSeconds;

            while (Time.time < endTime)
            {
                float fraction = (Time.time - startTime) / timeSeconds;
                image.alpha = Mathf.Lerp(start, end, fraction);

                await UniTask.NextFrame();
            }

            image.alpha = end;
        }

        public static async UniTask AnimateCount(this TextMeshProUGUI text, float start, float end, float timeSeconds)
        {
            float startTime = Time.time;
            float endTime = startTime + timeSeconds;

            while (Time.time < endTime)
            {
                float fraction = (Time.time - startTime) / timeSeconds;
                text.text = Mathf.Ceil(Mathf.Lerp(start, end, fraction)).ToString("0");

                await UniTask.NextFrame();
            }

            text.text = Mathf.Ceil(end).ToString("0");
        }
    }
}
