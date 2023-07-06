using System;
using UnityEngine;
using Utilities;
using Zenject;

namespace Game.Controllers
{
    public class SpeedController : IController, ITickable
    {
        public event Action<float> OnSpeedChanged;

        private float targetSpeed, currentSpeed;

        [Inject]
        private GameConfig gameConfig;

        public void SetSpeed(float speed)
        {
            targetSpeed = speed;
        }

        public void Tick()
        {
            if (Math.Abs(currentSpeed - targetSpeed) < Constants.FloatComparisonDelta)
                return;

            currentSpeed += Mathf.Sign(targetSpeed - currentSpeed) * gameConfig.acceleration * Time.deltaTime;

            OnSpeedChanged?.Invoke(currentSpeed);
        }
    }
}
