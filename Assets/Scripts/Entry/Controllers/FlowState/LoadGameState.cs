using System;
using Cysharp.Threading.Tasks;
using Entry.Models;
using Zenject;

namespace Entry.Controllers
{
    public class LoadGameState : FlowState
    {
        public Action OnStart;

        [Inject]
        private SceneLoader sceneLoader;

        public override async UniTask OnEnter()
        {
            OnStart?.Invoke();

            await sceneLoader.Unload(SceneName.Menu);
            await sceneLoader.Load(SceneName.Game);

            StateMachine.Stop();
        }
    }
}
