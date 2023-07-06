using Cysharp.Threading.Tasks;
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
    }
}
