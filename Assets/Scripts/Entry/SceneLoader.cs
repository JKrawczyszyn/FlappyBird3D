using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entry
{
    public class SceneLoader
    {
        public UniTask Load(string name)
        {
            Debug.Log($"Loading scene '{name}'.");

            return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive).ToUniTask();
        }

        public UniTask Unload(string name)
        {
            var scene = SceneManager.GetSceneByName(name);

            if (!scene.isLoaded)
                return UniTask.CompletedTask;

            Debug.Log($"Unloading scene '{name}'.");

            return SceneManager.UnloadSceneAsync(name).ToUniTask();
        }
    }
}
