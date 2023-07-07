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

        private readonly List<GameObject> elements = new();

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

            CreateElements(createCallback, assetName);
        }

        private void SpeedChanged(float speed)
        {
            moveSpeed = speed;
        }

        private void CreateElements(Func<string, Vector3, Transform, GameObject> createCallback, string assetName)
        {
            for (var i = 0; i < loopLength; i++)
            {
                var go = createCallback(assetName, i * Vector3.forward * interval, container);
                elements.Add(go);
            }
        }

        private void Update()
        {
            UpdateElementsPositions();
        }

        private void UpdateElementsPositions()
        {
            if (moveSpeed == 0f)
                return;

            var positionChange = -Vector3.forward * moveSpeed * Time.deltaTime;

            foreach (var element in elements)
            {
                element.transform.position += positionChange;

                if (element.transform.position.z < -interval)
                    element.transform.position += Vector3.forward * interval * loopLength;
            }
        }

        public void OnDestroy()
        {
            speedController.OnSpeedChanged -= SpeedChanged;
        }
    }
}
