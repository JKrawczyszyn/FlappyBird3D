using Cysharp.Threading.Tasks;
using Entry;
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
        public async UniTaskVoid Construct()
        {
            Debug.Log("Start GameEntryPoint.");

            gameplayController.Initialize();

            await assetsService.WaitForCache(assetsRepository.AssetNamesForScene(SceneName.Game));

            gameplayController.Start();
        }
    }
}
