using System.Linq;
using Cysharp.Threading.Tasks;
using Entry;
using Entry.Models;
using Entry.Services;
using Menu.Controllers;
using UnityEngine;
using Utilities;
using Zenject;

namespace Menu.Views
{
    public class PanelFactory
    {
        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsService assetsService;

        private Asset[] assets;

        [Inject]
        private async UniTaskVoid Construct()
        {
            assets = assetsRepository.AssetsForScene(SceneName.Menu);

            await assetsService.CacheReferences(assets.Select(a => a.name));
        }

        public IPanel Create(IPanelContext context, Transform container)
        {
            switch (context)
            {
                case HighScoresPanelContext highScoresPanelContext:
                {
                    var panelName = assets.FilterWithTag(AssetTag.HighScoresPanel).GetRandom().name;

                    var panel = assetsService.Instantiate<HighScoresPanel>(panelName, Vector3.zero, container);

                    panel.Initialize(highScoresPanelContext);

                    return panel;
                }
                case MainMenuPanelContext mainMenuPanelContext:
                {
                    var panelName = assets.FilterWithTag(AssetTag.MenuPanel).GetRandom().name;

                    var panel = assetsService.Instantiate<MenuPanel>(panelName, Vector3.zero, container);

                    panel.Initialize(mainMenuPanelContext);

                    return panel;
                }
                case SetHighScorePanelContext setHighScorePanelContext:
                {
                    var panelName = assets.FilterWithTag(AssetTag.EnterHighScorePanel).GetRandom().name;

                    var panel = assetsService.Instantiate<SetHighScorePanel>(panelName, Vector3.zero, container);

                    panel.Initialize(setHighScorePanelContext);

                    return panel;
                }
                default:
                    throw new System.NotImplementedException($"Unknown panel context '{context.GetType()}'.");
            }
        }
    }
}
