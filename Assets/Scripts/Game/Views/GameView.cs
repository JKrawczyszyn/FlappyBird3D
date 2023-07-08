using Cysharp.Threading.Tasks;
using Game.Controllers;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class GameView : MonoBehaviour
    {
        [Inject]
        private GameplayController gameplayController;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsProvider assetsProvider;

        private async void Start()
        {
            await StartGame();
        }

        private async UniTask StartGame()
        {
            await assetsProvider.WaitForCache(assetsRepository.AssetNamesForScene(SceneName.Game));

            gameplayController.StartGame();
        }
    }
}
