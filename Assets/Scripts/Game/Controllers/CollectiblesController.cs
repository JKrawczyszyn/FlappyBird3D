﻿using System;
using System.Collections.Generic;
using System.Linq;
using Entry;
using Entry.Models;
using Entry.Services;
using Game.Models;
using UnityEngine;
using Zenject;

namespace Game.Controllers
{
    public class CollectiblesController : IController
    {
        [Inject]
        private RandomService randomService;

        public event Action<CollectibleModel> OnAdd
        {
            add => movingObjectsController.OnAdd += value;
            remove => movingObjectsController.OnAdd -= value;
        }

        public event Action<CollectibleModel> OnRemove
        {
            add => movingObjectsController.OnRemove += value;
            remove => movingObjectsController.OnRemove -= value;
        }

        public event Action<List<CollectibleModel>> OnUpdatePositions
        {
            add => movingObjectsController.OnUpdatePositions += value;
            remove => movingObjectsController.OnUpdatePositions -= value;
        }

        [Inject]
        private MovingObjectsController<CollectibleModel> movingObjectsController;

        [Inject]
        private Config config;

        [Inject]
        private AssetsRepository assetsRepository;

        private int types;

        public void Initialize()
        {
            types = assetsRepository.AssetCount(AssetTag.Collectible);

            movingObjectsController.Initialize(config.collectiblesConfig, GetModel);
        }

        private CollectibleModel GetModel(float position)
        {
            var config = (CollectiblesConfig)movingObjectsController.Config;
            int id = IdProvider.GetNextId();
            int type = randomService.GetRandom(types);
            float xPosition = randomService.Range(config.spawnMin.x, config.spawnMax.x);
            float yPosition = randomService.Range(config.spawnMin.y, config.spawnMax.y);

            return new CollectibleModel(id, type, new Vector3(xPosition, yPosition, position));
        }

        public void Remove(int id)
        {
            CollectibleModel model = movingObjectsController.Models.First(m => m.Id == id);
            movingObjectsController.RemoveModel(model);
        }
    }
}
