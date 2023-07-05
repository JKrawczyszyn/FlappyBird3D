using System;
using UnityEngine;
using Zenject;

namespace Fp.Game.Controllers
{
    public class SpeedController : ITickable
    {
        private const float tolerance = 0.0001f;

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
            if (Math.Abs(currentSpeed - targetSpeed) < tolerance)
                return;

            currentSpeed += Mathf.Sign(targetSpeed - currentSpeed) * gameConfig.acceleration * Time.deltaTime;

            OnSpeedChanged?.Invoke(currentSpeed);
        }
    }
}
