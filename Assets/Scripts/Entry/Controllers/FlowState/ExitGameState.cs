using Cysharp.Threading.Tasks;
// ReSharper disable once RedundantUsingDirective
using UnityEngine;

namespace Entry.Controllers
{
    public class ExitGameState : FlowState
    {
        public override async UniTask OnEnter()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            await UniTask.Yield();
        }
    }
}
