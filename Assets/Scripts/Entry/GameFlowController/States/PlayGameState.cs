using Cysharp.Threading.Tasks;
using Utilities;
using Zenject;

namespace Entry.Controllers
{
    public class PlayGameState : GameFlowState
    {
        [Inject]
        private readonly SceneLoader sceneLoader;

        public override void Perform()
        {
            sceneLoader.Unload(Constants.MenuScene).Forget();
            sceneLoader.Load(Constants.GameScene).Forget();
        }
    }
}
