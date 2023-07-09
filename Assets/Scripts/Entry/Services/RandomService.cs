using System;
using System.Collections.Generic;
using System.Linq;
using Entry.Controllers;
using Zenject;
using Random = UnityEngine.Random;

namespace Entry.Services
{
    public class RandomService : IService, IDisposable
    {
        [Inject]
        private Config config;

        [Inject]
        private LoadGameState loadGameState;

        private Random.State state;

        [Inject]
        private void Construct()
        {
            loadGameState.OnStart += LoadGameStart;
        }

        private void LoadGameStart()
        {
            Random.InitState(config.gameplayConfig.seed != -1 ? config.gameplayConfig.seed : Environment.TickCount);

            state = Random.state;
        }

        public float Range(float min, float max)
        {
            Random.state = state;

            var result = Random.Range(min, max);

            state = Random.state;

            return result;
        }

        public int GetRandom(int value)
        {
            Random.state = state;

            var result = Random.Range(0, value);

            state = Random.state;

            return result;
        }

        public T GetRandom<T>(IEnumerable<T> collection)
        {
            T[] array = collection as T[] ?? collection.ToArray();

            Random.state = state;

            var result = array[Random.Range(0, array.Length)];

            state = Random.state;

            return result;
        }

        public void Dispose()
        {
            loadGameState.OnStart -= LoadGameStart;
        }
    }
}
