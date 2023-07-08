using Cysharp.Threading.Tasks;
using Entry.Models;
using Entry.Services;
using Game.Controllers;
using UnityEngine;
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
        private AssetsService assetsService;

        private async void Start()
        {
            await StartGame();
        }

        private async UniTask StartGame()
        {
            await assetsService.WaitForCache(assetsRepository.AssetNamesForScene(SceneName.Game));

            gameplayController.StartGame();
        }
    }
}
