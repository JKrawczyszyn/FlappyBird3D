using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Utilities
{
    // If more animations are required, it would be better to use some animation plugin, like DOTween.
    public static class AnimationsExtensions
    {
        public static async UniTask AnimateAlpha(this CanvasGroup image, float start, float end, float timeSeconds,
                                                 CancellationToken cancellationToken)
        {
            float startTime = Time.time;
            float endTime = startTime + timeSeconds;

            while (Time.time < endTime)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                float fraction = (Time.time - startTime) / timeSeconds;
                image.alpha = Mathf.Lerp(start, end, fraction);

                await UniTask.NextFrame(cancellationToken);
            }

            image.alpha = end;
        }

        public static async UniTask AnimateCount(this TextMeshProUGUI text, float start, float end, float timeSeconds,
                                                 CancellationToken cancellationToken)
        {
            float startTime = Time.time;
            float endTime = startTime + timeSeconds;

            while (Time.time < endTime)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                float fraction = (Time.time - startTime) / timeSeconds;
                text.text = Mathf.Ceil(Mathf.Lerp(start, end, fraction)).ToString("0");

                await UniTask.NextFrame(cancellationToken);
            }

            text.text = Mathf.Ceil(end).ToString("0");
        }
    }
}
