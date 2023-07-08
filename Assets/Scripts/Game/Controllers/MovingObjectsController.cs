using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models;
using UnityEngine;
using Zenject;

namespace Game.Controllers
{
    public class MovingObjectsController<T> : ITickable where T : class, IMovingObjectModel
    {
        public event Action<T> OnAdd;
        public event Action<T> OnRemove;
        public event Action<List<T>> OnUpdatePositions;

        [Inject]
        private SpeedController speedController;

        private readonly List<T> models = new();
        public IEnumerable<T> Models => models.AsReadOnly();

        public MovingObjectsConfig Config { get; private set; }
        private Func<float, T> modelGetter;

        public float MoveSpeed { get; private set; }

        private int count;

        [Inject]
        private void Construct()
        {
            speedController.OnSpeedChanged += SpeedChanged;
        }

        public void Initialize(MovingObjectsConfig config, Func<float, T> modelGetter)
        {
            Config = config;
            this.modelGetter = modelGetter;

            CreateStartModels();
        }

        private void SpeedChanged(float speed)
        {
            MoveSpeed = speed;
        }

        private void CreateStartModels()
        {
            int currentPosition = Config.startSpawnDistance;

            while (currentPosition < Config.maxSpawnDistance)
            {
                AddModel(currentPosition);

                count++;

                currentPosition += Config.intervalDistance;
            }
        }

        public void Tick()
        {
            if (MoveSpeed == 0f && models.Count > 0)
                return;

            UpdatePositions();
            UpdateRemove();
            UpdateAdd();
        }

        private void UpdatePositions()
        {
            float positionChange = -MoveSpeed * Time.deltaTime;

            foreach (T model in models)
                model.PositionZ += positionChange;

            OnUpdatePositions?.Invoke(models);
        }

        private void UpdateRemove()
        {
            IEnumerable<T> toDestroy = models.Where(e => !IsInsideOfView(e.PositionZ));
            foreach (T model in toDestroy.ToArray())
                RemoveModel(model);
        }

        private void UpdateAdd()
        {
            if (models.Count >= count)
                return;

            AddModel(models[^1].PositionZ + Config.intervalDistance);
        }

        public void RemoveModel(T model)
        {
            models.Remove(model);

            OnRemove?.Invoke(model);
        }

        private void AddModel(float position)
        {
            T model = modelGetter(position);
            models.Add(model);

            OnAdd?.Invoke(model);
        }

        private bool IsInsideOfView(float position) => position > 0f;
    }
}
