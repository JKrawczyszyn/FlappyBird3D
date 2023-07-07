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
            obstaclesController.OnAddElement += AddElement;
            obstaclesController.OnRemoveElement += RemoveElement;
            obstaclesController.OnUpdatePositions += UpdatePositions;

            assetNames = assetsRepository.AssetNames(AssetTag.Obstacle);

            await assetsProvider.CacheReferences<Obstacle>(assetNames);

            obstaclesController.Initialize();
        }

        private void UpdatePositions(List<ObstacleModel> models)
        {
            foreach (var model in models)
                elements[model.Id].transform.position = Vector3.forward * model.Position;
        }

        private void RemoveElement(ObstacleModel model)
        {
            assetsProvider.Release(elements[model.Id]);
            elements.Remove(model.Id);
        }

        private void AddElement(ObstacleModel model)
        {
            var assetName = assetNames[model.Type];
            var element = assetsProvider.Instantiate<Obstacle>(assetName, Vector3.forward * model.Position, container);
            elements.Add(model.Id, element);
        }

        private void OnDestroy()
        {
            obstaclesController.OnAddElement -= AddElement;
            obstaclesController.OnRemoveElement -= RemoveElement;
            obstaclesController.OnUpdatePositions -= UpdatePositions;
        }
    }
}
