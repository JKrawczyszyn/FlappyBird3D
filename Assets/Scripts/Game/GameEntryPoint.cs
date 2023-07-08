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

        [Inject]
        private GameConfig gameConfig;

        private async void Start()
        {
            Debug.Log("Start GameEntryPoint.");

            SetParameters();

            await Initialize();
        }

        private void SetParameters()
        {
            Application.targetFrameRate = gameConfig.frameRate;
            Physics.gravity = Vector3.down * gameConfig.gravity;
        }

        private async UniTask Initialize()
        {
            await assetsService.WaitForCache(assetsRepository.AssetNamesForScene(SceneName.Game));

            gameplayController.Initialize();
        }
    }
}
