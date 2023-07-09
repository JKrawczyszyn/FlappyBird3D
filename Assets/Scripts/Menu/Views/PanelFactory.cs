using System.Linq;
using Cysharp.Threading.Tasks;
using Entry;
using Entry.Models;
using Entry.Services;
using Menu.Controllers;
using UnityEngine;
using Zenject;

namespace Menu.Views
{
    public class PanelFactory
    {
        [Inject]
        private RandomService randomService;

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
                    var panelName = randomService.GetRandom(assets.FilterWithTag(AssetTag.HighScoresPanel)).name;

                    var panel = assetsService.Instantiate<HighScoresPanel>(panelName, Vector3.zero, container);

                    panel.Initialize(highScoresPanelContext);

                    return panel;
                }
                case MainMenuPanelContext mainMenuPanelContext:
                {
                    var panelName = randomService.GetRandom(assets.FilterWithTag(AssetTag.MenuPanel)).name;

                    var panel = assetsService.Instantiate<MenuPanel>(panelName, Vector3.zero, container);

                    panel.Initialize(mainMenuPanelContext);

                    return panel;
                }
                case SetHighScorePanelContext setHighScorePanelContext:
                {
                    var panelName = randomService.GetRandom(assets.FilterWithTag(AssetTag.EnterHighScorePanel)).name;

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
