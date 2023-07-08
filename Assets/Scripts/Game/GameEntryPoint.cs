using Cysharp.Threading.Tasks;
using Entry.Models;
using Entry.Services;
using Game.Controllers;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameEntryPoint : MonoBehaviour
    {
        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsService assetsService;

        [Inject]
        private GameplayController gameplayController;

        private async void Start()
        {
            Debug.Log("Start GameEntryPoint.");

            await Initialize();
        }

        private async UniTask Initialize()
        {
            await assetsService.WaitForCache(assetsRepository.AssetNamesForScene(SceneName.Game));

            gameplayController.Initialize();
        }
    }
}
