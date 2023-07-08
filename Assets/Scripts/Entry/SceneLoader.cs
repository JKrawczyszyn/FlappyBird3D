using System;
using Cysharp.Threading.Tasks;
using Entry.Models;
using Entry.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Entry
{
    public class SceneLoader
    {
        public event Func<UniTask> OnSceneLoadEnd;
        public event Func<UniTask> OnSceneUnloadStart;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsService assetsService;

        public async UniTask Load(SceneName name)
        {
            var nameString = name.ToString();

            Debug.Log($"Loading scene '{nameString}'.");

            await SceneManager.LoadSceneAsync(nameString, LoadSceneMode.Additive);

            await assetsService.WaitForCache(assetsRepository.AssetNamesForScene(name));

            if (OnSceneLoadEnd != null)
                await OnSceneLoadEnd();
        }

        public async UniTask Unload(SceneName name)
        {
            var nameString = name.ToString();

            Scene scene = SceneManager.GetSceneByName(nameString);
            if (!scene.isLoaded)
                return;

            Debug.Log($"Unloading scene '{nameString}'.");

            if (OnSceneUnloadStart != null)
                await OnSceneUnloadStart();

            assetsService.ClearPools();

            await SceneManager.UnloadSceneAsync(nameString);
        }
    }
}
