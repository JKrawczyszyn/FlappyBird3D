using Cysharp.Threading.Tasks;
using Zenject;

namespace Entry.Controllers
{
    public class LoadGameState : FlowState
    {
        [Inject]
        private SceneLoader sceneLoader;

        public override async UniTask OnEnter()
        {
            await sceneLoader.Unload(SceneName.Menu);
            await sceneLoader.Load(SceneName.Game);

            StateMachine.Stop();
        }
    }
}
