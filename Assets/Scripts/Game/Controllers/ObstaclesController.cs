using System;
using System.Collections.Generic;
using System.Linq;
using Game.Views;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;
using Zenject;

namespace Game.Controllers
{
    public class ObstaclesController : IController, ITickable
    {
        public event Action<ObstacleModel> OnAddElement;
        public event Action<ObstacleModel> OnRemoveElement;
        public event Action<List<ObstacleModel>> OnUpdatePositions;

        [Inject]
        private GameConfig gameConfig;

        [Inject]
        private SpeedController speedController;

        [Inject]
        private BirdController birdController;

        [Inject]
        private PlayState playState;

        [Inject]
        private AssetsRepository assetsRepository;

        private ViewState state;

        private float moveSpeed;

        private readonly List<ObstacleModel> elements = new();

        private ObstaclesConfig Config => gameConfig.obstaclesConfig;

        private int Count => Config.spawnDistance / Config.intervalDistance;

        private int types;

        [Inject]
        private void Construct()
        {
            playState.OnStart += StartMove;
            speedController.OnSpeedChanged += SpeedChanged;
        }

        public void Initialize()
        {
            types = assetsRepository.AssetCount(AssetTag.Obstacle);

            CreateStartElements();

            state = ViewState.Initialized;
        }

        private void StartMove()
        {
            Assert.IsTrue(state == ViewState.Initialized, "Not initialized.");

            state = ViewState.Started;
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

                AddElement(currentPosition);//, assetNames.GetRandom());
            }
        }

        public void Tick()
        {
            UpdateElements();
        }

        private void UpdateElements()
        {
            if (state != ViewState.Started)
                return;

            Assert.IsTrue(elements.Count > 0, "Elements count must be greater than zero.");

            UpdatePositions();
            UpdateRemove();
            UpdateAdd();
        }

        private void UpdatePositions()
        {
            var positionChange = -moveSpeed * Time.deltaTime;

            foreach (var element in elements)
                element.Position += positionChange;

            OnUpdatePositions?.Invoke(elements);
        }

        private void UpdateRemove()
        {
            var toDestroy = elements.Where(e => !IsInsideOfView(e.Position));
            foreach (var element in toDestroy.ToArray())
                RemoveElement(element);
        }

        private void UpdateAdd()
        {
            if (elements.Count >= Count)
                return;

            AddElement(elements[^1].Position + Config.intervalDistance);
        }

        private void RemoveElement(ObstacleModel model)
        {
            elements.Remove(model);

            OnRemoveElement?.Invoke(model);
        }

        private void AddElement(float position)
        {
            var id = IdProvider.GetNextId();
            var type = types.GetRandom();

            var model = new ObstacleModel(id, type, position);
            elements.Add(model);

            OnAddElement?.Invoke(model);
        }

        private bool IsInsideOfView(float position) => position > 0f;

        private bool IsInsideOfFreeDistance(int position) => position < Config.freeDistance;
    }
}
