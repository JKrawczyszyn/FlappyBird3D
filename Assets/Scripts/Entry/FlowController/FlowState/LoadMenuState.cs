using Cysharp.Threading.Tasks;
using Zenject;

namespace Entry.Controllers
{
    public class LoadMenuState : FlowState
    {
        [Inject]
        private SceneLoader sceneLoader;

        public override async UniTask OnEnter()
        {
            await sceneLoader.Unload(SceneName.Game);
            await sceneLoader.Load(SceneName.Menu);

            StateMachine.Stop();
        }
    }
}
