using Cysharp.Threading.Tasks;
using Game.Controllers;
using TMPro;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class UIView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup loadingPanel;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        [Inject]
        private GameInputController gameInputController;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private GameController gameController;

        [Inject]
        private AssetsProvider assetsProvider;

        private void Start()
        {
            Initialize().Forget();
        }

        private async UniTaskVoid Initialize()
        {
            await assetsProvider.WaitForCache(
                assetsRepository.AssetNames(new[] { AssetTag.Bird, AssetTag.Walls, AssetTag.Obstacle, }));

            await loadingPanel.AnimateAlpha(1f, 0f, 0.2f);

            loadingPanel.gameObject.SetActive(false);

            gameInputController.OnInteract += Interact;
            gameInputController.InteractionEnable();
        }

        private void Interact()
        {
            gameController.StartGame();
            gameInputController.InteractionDisable();
        }

        public void OnDestroy()
        {
            gameInputController.OnInteract -= Interact;
        }
    }
}
