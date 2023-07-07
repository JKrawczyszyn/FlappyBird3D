using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;
using Zenject;

namespace Game.Controllers
{
    public class ObstaclesController : IController, ITickable
    {
        public event Action<ObstacleModel> OnAdd;
        public event Action<ObstacleModel> OnRemove;
        public event Action<List<ObstacleModel>> OnUpdatePositions;
        public event Action<ObstacleModel> OnPassed;

        [Inject]
        private GameConfig gameConfig;

        [Inject]
        private SpeedController speedController;

        [Inject]
        private AssetsRepository assetsRepository;

        private float moveSpeed;

        private readonly List<ObstacleModel> models = new();

        private ObstaclesConfig Config => gameConfig.obstaclesConfig;

        private int Count => Config.spawnDistance / Config.intervalDistance;

        private int types;

        [Inject]
        private void Construct()
        {
            speedController.OnSpeedChanged += SpeedChanged;
        }

        public void Initialize()
        {
            types = assetsRepository.AssetCount(AssetTag.Obstacle);

            CreateStartElements();
        }

        private void SpeedChanged(float speed)
        {
            moveSpeed = speed;
        }

        private void CreateStartElements()
        {
            for (var i = 0; i < Count; i++)
            {
                int currentPosition = Config.intervalDistance * i;

                if (!IsInsideOfView(currentPosition))
                    continue;

                if (IsInsideOfFreeDistance(currentPosition))
                    continue;

                AddModel(currentPosition);
            }
        }

        public void Tick()
        {
            UpdateModels();
        }

        private void UpdateModels()
        {
            if (moveSpeed == 0f)
                return;

            Assert.IsTrue(models.Count > 0, "Elements count must be greater than zero.");

            UpdatePositions();
            UpdateRemove();
            UpdateAdd();
            UpdatePassed();
        }

        private void UpdatePositions()
        {
            var positionChange = -moveSpeed * Time.deltaTime;

            foreach (var model in models)
                model.Position += positionChange;

            OnUpdatePositions?.Invoke(models);
        }

        private void UpdateRemove()
        {
            var toDestroy = models.Where(e => !IsInsideOfView(e.Position));

            foreach (var model in toDestroy.ToArray())
                RemoveModel(model);
        }

        private void UpdateAdd()
        {
            if (models.Count >= Count)
                return;

            AddModel(models[^1].Position + Config.intervalDistance);
        }

        private void UpdatePassed()
        {
            foreach (var model in models)
                if (!model.IsPassed && model.Position < gameConfig.birdConfig.startPosition.z)
                {
                    model.IsPassed = true;

                    OnPassed?.Invoke(model);
                }
        }

        private void RemoveModel(ObstacleModel model)
        {
            models.Remove(model);

            OnRemove?.Invoke(model);
        }

        private void AddModel(float position)
        {
            var id = IdProvider.GetNextId();
            var type = types.GetRandom();

            var model = new ObstacleModel(id, type, position);
            models.Add(model);

            OnAdd?.Invoke(model);
        }

        private bool IsInsideOfView(float position) => position > 0f;

        private bool IsInsideOfFreeDistance(int position) => position < Config.freeDistance;
    }
}
