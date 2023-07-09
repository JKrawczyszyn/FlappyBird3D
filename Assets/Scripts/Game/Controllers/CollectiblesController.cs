using System;
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

        public event Action<CollectibleModel> OnScore;

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

        public void Start()
        {
            movingObjectsController.Start();
        }

        private CollectibleModel GetModel(float position)
        {
            var cfg = config.collectiblesConfig;
            int id = IdProvider.GetNextId();
            int type = randomService.GetRandom(types);
            float xPosition = randomService.Range(cfg.spawnMin.x, cfg.spawnMax.x);
            float yPosition = randomService.Range(cfg.spawnMin.y, cfg.spawnMax.y);

            return new CollectibleModel(id, type, new Vector3(xPosition, yPosition, position));
        }

        public void Score(int id)
        {
            CollectibleModel model = movingObjectsController.Models.First(m => m.Id == id);
            OnScore?.Invoke(model);
        }
    }
}
