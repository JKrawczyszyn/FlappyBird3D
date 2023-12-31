using System;
using System.Collections.Generic;
using Entry;
using Entry.Models;
using Entry.Services;
using Game.Models;
using Zenject;

namespace Game.Controllers
{
    public class ObstaclesController : IController, ITickable
    {
        [Inject]
        private RandomService randomService;

        public event Action<ObstacleModel> OnAdd
        {
            add => movingObjectsController.OnAdd += value;
            remove => movingObjectsController.OnAdd -= value;
        }

        public event Action<ObstacleModel> OnRemove
        {
            add => movingObjectsController.OnRemove += value;
            remove => movingObjectsController.OnRemove -= value;
        }

        public event Action<List<ObstacleModel>> OnUpdatePositions
        {
            add => movingObjectsController.OnUpdatePositions += value;
            remove => movingObjectsController.OnUpdatePositions -= value;
        }

        public event Action<ObstacleModel> OnPassedPlayer;

        [Inject]
        private MovingObjectsController<ObstacleModel> movingObjectsController;

        [Inject]
        private Config config;

        [Inject]
        private AssetsRepository assetsRepository;

        private int types;

        public void Initialize()
        {
            types = assetsRepository.AssetCount(AssetTag.Obstacle);

            movingObjectsController.Initialize(config.obstaclesConfig, GetModel);
        }

        public void Start()
        {
            movingObjectsController.Start();
        }

        private ObstacleModel GetModel(float position)
        {
            int id = IdProvider.GetNextId();
            int type = randomService.GetRandom(types);

            return new ObstacleModel(id, type, position);
        }

        public void Tick()
        {
            if (movingObjectsController.MoveSpeed == 0f)
                return;

            UpdatePassed();
        }

        private void UpdatePassed()
        {
            foreach (ObstacleModel model in movingObjectsController.Models)
            {
                if (model.IsPassed || model.Position > config.birdConfig.startPosition.z)
                    continue;

                model.IsPassed = true;

                OnPassedPlayer?.Invoke(model);
            }
        }
    }
}
