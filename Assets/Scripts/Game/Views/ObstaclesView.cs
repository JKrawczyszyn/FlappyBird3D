using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Controllers;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class ObstaclesView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private GameConfig gameConfig;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsProvider assetsProvider;

        [Inject]
        private ObstaclesController obstaclesController;

        private string[] assetNames;

        private readonly Dictionary<int, Obstacle> elements = new();

        [Inject]
        private async UniTaskVoid Construct()
        {
            obstaclesController.OnAdd += Add;
            obstaclesController.OnRemove += Remove;
            obstaclesController.OnUpdatePositions += UpdatePositions;
            obstaclesController.OnPassedPlayer += PassedPlayer;

            assetNames = assetsRepository.AssetNames(AssetTag.Obstacle);

            await assetsProvider.CacheReferences<Obstacle>(assetNames);

            obstaclesController.Initialize();
        }

        private void Add(ObstacleModel model)
        {
            var assetName = assetNames[model.Type];
            var element = assetsProvider.Instantiate<Obstacle>(assetName, Vector3.forward * model.Position, container);
            element.SetAlpha(1f);
            elements.Add(model.Id, element);
        }

        private void Remove(ObstacleModel model)
        {
            assetsProvider.Release(elements[model.Id]);
            elements.Remove(model.Id);
        }

        private void UpdatePositions(List<ObstacleModel> models)
        {
            foreach (var model in models)
                elements[model.Id].transform.position = Vector3.forward * model.Position;
        }

        private void PassedPlayer(ObstacleModel model)
        {
            elements[model.Id].SetAlpha(gameConfig.obstaclesConfig.behindAlpha);
        }

        private void OnDestroy()
        {
            obstaclesController.OnAdd -= Add;
            obstaclesController.OnRemove -= Remove;
            obstaclesController.OnUpdatePositions -= UpdatePositions;
            obstaclesController.OnPassedPlayer -= PassedPlayer;
        }
    }
}
