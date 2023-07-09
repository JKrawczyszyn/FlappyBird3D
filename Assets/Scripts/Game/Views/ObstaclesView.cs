using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Entry;
using Entry.Models;
using Entry.Services;
using Game.Controllers;
using Game.Models;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class ObstaclesView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private Config config;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsService assetsService;

        [Inject]
        private ObstaclesController obstaclesController;

        private string[] assetNames;

        private readonly Dictionary<int, Obstacle> instances = new();

        [Inject]
        private async UniTaskVoid Construct()
        {
            obstaclesController.OnAdd += Add;
            obstaclesController.OnRemove += Remove;
            obstaclesController.OnUpdatePositions += UpdatePositions;
            obstaclesController.OnPassedPlayer += PassedPlayer;
            obstaclesController.Initialize();

            assetNames = assetsRepository.AssetNames(AssetTag.Obstacle);

            await assetsService.CacheReferences(assetNames);

            obstaclesController.Start();
        }

        private void Add(ObstacleModel model)
        {
            var assetName = assetNames[model.Type];
            var instance = assetsService.Instantiate<Obstacle>(assetName, Vector3.forward * model.Position, container);
            instance.SetAlpha(1f);
            instances.Add(model.Id, instance);
        }

        private void Remove(ObstacleModel model)
        {
            assetsService.Release(instances[model.Id]);
            instances.Remove(model.Id);
        }

        private void UpdatePositions(List<ObstacleModel> models)
        {
            foreach (var model in models)
                instances[model.Id].transform.position = Vector3.forward * model.Position;
        }

        private void PassedPlayer(ObstacleModel model)
        {
            instances[model.Id].SetAlpha(config.obstaclesConfig.behindAlpha);
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
