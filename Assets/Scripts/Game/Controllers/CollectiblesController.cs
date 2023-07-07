using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Controllers
{
    public class CollectiblesController : IController
    {
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
        private GameConfig gameConfig;

        public void Initialize()
        {
            movingObjectsController.Initialize(gameConfig.collectiblesConfig, GetModel);
        }

        private CollectibleModel GetModel(float position)
        {
            var config = (CollectiblesConfig)movingObjectsController.Config;
            var id = IdProvider.GetNextId();
            var xPosition = Random.Range(config.spawnMin.x, config.spawnMax.x);
            var yPosition = Random.Range(config.spawnMin.y, config.spawnMax.y);

            return new CollectibleModel(id, new Vector3(xPosition, yPosition, position));
        }

        public void Remove(int id)
        {
            var model = movingObjectsController.Models.First(m => m.Id == id);
            movingObjectsController.RemoveModel(model);
        }
    }
}
