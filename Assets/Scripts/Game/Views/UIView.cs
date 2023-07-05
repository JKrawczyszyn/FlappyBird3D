using Cysharp.Threading.Tasks;
using Fp.Game.Controllers;
using Fp.Utilities.Assets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Fp.Game.Views
{
    public class UIView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup loadingPanel;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        [Inject]
        private GameControls gameControls;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private GameController gameController;

        [Inject]
        private AssetsProvider assetsProvider;

        [Inject]
        public async UniTaskVoid Construct()
        {
            await assetsProvider.WaitForCache(
                assetsRepository.AssetNames(new[] { AssetTag.Bird, AssetTag.Walls, AssetTag.Obstacle, }));

            await loadingPanel.AnimateAlpha(1f, 0f, 0.2f);

            loadingPanel.gameObject.SetActive(false);

            gameControls.Map.Interact.performed += Interact;
            gameControls.Map.Interact.Enable();
        }

        private void Interact(InputAction.CallbackContext _)
        {
            gameController.StartGame();
        }

        private void OnDestroy()
        {
            gameControls.Map.Interact.performed -= Interact;
        }
    }
}
