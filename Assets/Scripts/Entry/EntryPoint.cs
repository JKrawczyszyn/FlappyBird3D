using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;
using Zenject;

namespace Entry
{
    public class EntryPoint : MonoBehaviour
    {
        [Inject]
        private SceneLoader sceneLoader;

        private void Start()
        {
            Debug.Log("Start EntryPoint.");

            sceneLoader.Load(Constants.MenuScene).Forget();
        }
    }
}
