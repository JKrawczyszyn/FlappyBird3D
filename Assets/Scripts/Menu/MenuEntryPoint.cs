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

        [Inject]
        public async UniTaskVoid Construct()
        {
            Debug.Log("Start MenuEntryPoint.");

            await assetsService.WaitForCache(assetsRepository.AssetNamesForScene(SceneName.Menu));

            menuController.Start();
        }
    }
}
