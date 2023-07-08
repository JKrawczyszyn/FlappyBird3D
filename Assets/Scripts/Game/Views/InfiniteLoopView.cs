using System;
using System.Collections.Generic;
using Game.Controllers;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class InfiniteLoopView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [Inject]
        private SpeedController speedController;

        private float moveSpeed;

        private int interval;
        private int loopLength;

        private readonly List<GameObject> instances = new();

        [Inject]
        private void Construct()
        {
            speedController.OnSpeedChanged += SpeedChanged;
        }

        public void Initialize(string assetName,
                               int interval,
                               int loopLength,
                               Func<string, Vector3, Transform, GameObject> createCallback)
        {
            this.interval = interval;
            this.loopLength = loopLength;

            CreateInstances(createCallback, assetName);
        }

        private void SpeedChanged(float speed)
        {
            moveSpeed = speed;
        }

        private void CreateInstances(Func<string, Vector3, Transform, GameObject> createCallback, string assetName)
        {
            for (var i = 0; i < loopLength; i++)
            {
                var instance = createCallback(assetName, i * Vector3.forward * interval, container);
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

            foreach (var go in instances)
            {
                go.transform.position += positionChange;

                if (go.transform.position.z < -interval)
                    go.transform.position += Vector3.forward * interval * loopLength;
            }
        }

        public void OnDestroy()
        {
            speedController.OnSpeedChanged -= SpeedChanged;
        }
    }
}
