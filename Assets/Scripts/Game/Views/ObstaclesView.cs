using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Controllers;
using UnityEngine;
using UnityEngine.Assertions;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class ObstaclesView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private Camera camera;

        [Inject]
        private GameConfig gameConfig;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsProvider assetsProvider;

        [Inject]
        private GameController gameController;

        [Inject]
        private SpeedController speedController;

        private bool initialized;
        private bool started;

        private float moveSpeed;

        private string[] assetNames;

        private readonly List<Obstacle> elements = new();

        private ObstaclesConfig Config => gameConfig.obstaclesConfig;

        private int Count => Config.spawnDistance / Config.intervalDistance;

        private void Start()
        {
            Initialize().Forget();
        }

        private async UniTaskVoid Initialize()
        {
            assetNames = assetsRepository.AssetNames(AssetTag.Obstacle);

            await assetsProvider.CacheReferences<Obstacle>(assetNames);

            gameController.OnGameStarted += StartMove;
            speedController.OnSpeedChanged += SpeedChanged;

            moveSpeed = Config.startSpeed;

            CreateStartElements();

            initialized = true;
        }

        private void SpeedChanged(float speed)
        {
            moveSpeed = speed;
        }

        private void CreateStartElements()
        {
            for (var i = 0; i < Count; i++)
            {
                var currentPosition = Vector3.forward * Config.intervalDistance * i;

                if (!IsInsideOfView(currentPosition))
                    continue;

                if (IsInsideOfFreeDistance(currentPosition))
                    continue;

                AddElement(currentPosition, assetNames.GetRandom());
            }
        }

        private void Update()
        {
            UpdateElements();
        }

        private void UpdateElements()
        {
            if (!initialized || !started)
                return;

            Assert.IsTrue(elements.Count > 0, "Elements count must be greater than zero.");

            UpdatePositions();
            UpdateRemove();
            UpdateAdd();
        }

        private void UpdatePositions()
        {
            var positionChange = -Vector3.forward * moveSpeed * Time.deltaTime;

            foreach (var element in elements)
                element.transform.position += positionChange;
        }

        private void UpdateRemove()
        {
            var toDestroy = elements.Where(e => !IsInsideOfView(e.transform.position));
            foreach (var element in toDestroy.ToArray())
                RemoveElement(element);
        }

        private bool IsInsideOfView(Vector3 position)
        {
            var viewportPoint = camera.WorldToViewportPoint(position);
            return viewportPoint.z > 0;
        }

        private bool IsInsideOfFreeDistance(Vector3 position) => position.z < Config.freeDistance;

        private void UpdateAdd()
        {
            if (elements.Count >= Count)
                return;

            AddElement(elements[^1].transform.position + (Vector3.forward * Config.intervalDistance),
                       assetNames.GetRandom());
        }

        private void RemoveElement(Obstacle element)
        {
            assetsProvider.Release(element);
            elements.Remove(element);
        }

        private void AddElement(Vector3 position, string name)
        {
            var element = assetsProvider.Instantiate<Obstacle>(name, position, container);
            elements.Add(element);
        }

        private void StartMove()
        {
            Assert.IsTrue(initialized, "Not initialized.");

            started = true;
        }

        private void OnDestroy()
        {
            gameController.OnGameStarted -= StartMove;
            speedController.OnSpeedChanged -= SpeedChanged;
        }
    }
}
