using System;
using Game.Models;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Controllers
{
    public class SpeedController : IController, ITickable, IDisposable
    {
        public event Action<float> OnSpeedChanged;

        private float targetSpeed, currentSpeed;

        [Inject]
        private GameConfig gameConfig;

        [Inject]
        private ObstaclesController obstaclesController;

        private int passedCounter;

        [Inject]
        private void Construct()
        {
            obstaclesController.OnPassedPlayer += PassedObstacle;

            passedCounter = 0;
        }

        private void PassedObstacle(ObstacleModel _)
        {
            passedCounter++;

            if (passedCounter % gameConfig.gameplayConfig.speedUpInterval != 0)
                return;

            targetSpeed += gameConfig.gameplayConfig.speedUpValue;

            Debug.Log($"Speed up to '{targetSpeed}'.");
        }

        public void SetSpeed(float speed)
        {
            targetSpeed = speed;
        }

        public void Tick()
        {
            if (Math.Abs(currentSpeed - targetSpeed) < Constants.FloatComparisonDelta)
                return;

            currentSpeed += Mathf.Sign(targetSpeed - currentSpeed)
                            * gameConfig.gameplayConfig.speedUpAcceleration
                            * Time.deltaTime;

            OnSpeedChanged?.Invoke(currentSpeed);
        }

        public void Dispose()
        {
            obstaclesController.OnPassedPlayer -= PassedObstacle;
        }
    }
}
