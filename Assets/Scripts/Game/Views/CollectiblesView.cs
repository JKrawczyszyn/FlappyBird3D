﻿using System.Collections.Generic;
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
    public class CollectiblesView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsService assetsService;

        [Inject]
        private CollectiblesController collectiblesController;

        private string[] assetNames;

        private readonly Dictionary<int, Collectible> instances = new();

        [Inject]
        private async UniTaskVoid Construct()
        {
            collectiblesController.OnAdd += Add;
            collectiblesController.OnRemove += Remove;
            collectiblesController.OnScore += Score;
            collectiblesController.OnUpdatePositions += UpdatePositions;
            collectiblesController.Initialize();

            assetNames = assetsRepository.AssetNames(AssetTag.Collectible);

            await assetsService.CacheReferences(assetNames);

            collectiblesController.Start();
        }

        public int GetId(Collectible collectible)
        {
            // Can be optimized if necessary by using reverse/bidirectional dictionary.
            foreach ((int id, Collectible instance) in instances)
                if (instance == collectible)
                    return id;

            return -1;
        }

        private void Add(CollectibleModel model)
        {
            var assetName = assetNames[model.Type];
            var instance = assetsService.Instantiate<Collectible>(assetName, model.Position, container);
            instances.Add(model.Id, instance);
        }

        private void Remove(CollectibleModel model)
        {
            Collectible instance = instances[model.Id];

            instances.Remove(model.Id);

            assetsService.Release(instance);
        }

        private void Score(CollectibleModel model)
        {
            ScoreAsync(model).Forget();
        }

        private async UniTask ScoreAsync(CollectibleModel model)
        {
            Collectible instance = instances[model.Id];

            await instance.Collect();

            instances.Remove(model.Id);

            assetsService.Release(instance);
        }

        private void UpdatePositions(List<CollectibleModel> models)
        {
            foreach (var model in models)
                instances[model.Id].transform.position = model.Position;
        }

        private void OnDestroy()
        {
            collectiblesController.OnAdd -= Add;
            collectiblesController.OnRemove -= Remove;
            collectiblesController.OnScore -= Score;
            collectiblesController.OnUpdatePositions -= UpdatePositions;
        }
    }
}
