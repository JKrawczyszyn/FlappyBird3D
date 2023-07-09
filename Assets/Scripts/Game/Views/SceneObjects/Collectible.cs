using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Views
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField]
        private GameObject model;

        [SerializeField]
        private Collider collider;

        [SerializeField]
        private ParticleSystem particleSystem;

        private void Awake()
        {
            model.SetActive(true);
        }

        public async UniTask Collect()
        {
            model.SetActive(false);

            collider.enabled = false;

            particleSystem.Play();

            await UniTask.WaitWhile(() => particleSystem.isPlaying,
                                    cancellationToken: gameObject.GetCancellationTokenOnDestroy());
        }
    }
}
