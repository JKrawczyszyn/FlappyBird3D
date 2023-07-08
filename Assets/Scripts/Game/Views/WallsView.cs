using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Entry.Models;
using Entry.Services;
using Game.Controllers;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Views
{
    public class WallsView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private AssetsRepository assetsRepository;

        [Inject]
        private AssetsService assetsService;

        [Inject]
        private SpeedController speedController;

        [Inject]
        private Config config;

        private float moveSpeed;

        private string[] assetNames;

        private readonly List<Walls> instances = new();

        private WallsConfig Config => config.wallsConfig;

        [Inject]
        private async UniTaskVoid Construct()
        {
            speedController.OnSpeedChanged += SpeedChanged;

            assetNames = assetsRepository.AssetNames(AssetTag.Walls);

            await assetsService.CacheReferences(assetNames);

            CreateInstances();
        }

        private void SpeedChanged(float speed)
        {
            moveSpeed = speed;
        }

        private void CreateInstances()
        {
            for (var i = 0; i < Config.loop; i++)
            {
                var assetName = assetNames.GetRandom();
                var position = i * Vector3.forward * Config.interval;
                var instance = assetsService.Instantiate<Walls>(assetName, position, container);
                instances.Add(instance);
            }
        }

        private void Update()
        {
            UpdateInstancesPositions();
        }

        private void UpdateInstancesPositions()
        {
            if (moveSpeed == 0f)
                return;

            var positionChange = -Vector3.forward * moveSpeed * Time.deltaTime;

            foreach (var instance in instances)
            {
                instance.transform.position += positionChange;

                if (instance.transform.position.z < -Config.interval)
                    instance.transform.position += Vector3.forward * Config.interval * Config.loop;
            }
        }

        public void OnDestroy()
        {
            speedController.OnSpeedChanged -= SpeedChanged;
        }
    }
}
