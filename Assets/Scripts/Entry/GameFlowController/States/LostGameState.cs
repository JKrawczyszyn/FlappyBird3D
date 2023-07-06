using Cysharp.Threading.Tasks;
using Utilities;
using Zenject;

namespace Entry.Controllers
{
    public class LostGameState : GameFlowState
    {
        [Inject]
        private readonly SceneLoader sceneLoader;

        public override void Perform()
        {
            sceneLoader.Unload(Constants.GameScene).Forget();
            sceneLoader.Load(Constants.MenuScene).Forget();
        }
    }
}
