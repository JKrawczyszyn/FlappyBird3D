using Cysharp.Threading.Tasks;
using Entry;
using Entry.Models;
using Entry.Services;
using Menu.Controllers;
using UnityEngine;
using Zenject;

namespace Menu
{
    public class MenuEntryPoint : MonoBehaviour
    {
        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsService assetsService;

        [Inject]
        private MenuController menuController;

        private async void Start()
        {
            Debug.Log("Start MenuEntryPoint.");

            await Initialize();
        }

        private async UniTask Initialize()
        {
            await assetsService.WaitForCache(assetsRepository.AssetNamesForScene(SceneName.Menu));

            menuController.Initialize();
        }
    }
}
