using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Utilities.FSM
{
    public class StateMachine<T> where T : class, IState
    {
        [Inject]
        private readonly DiContainer container;

        public T CurrentState { get; private set; }

        private readonly Dictionary<Type, T> states = new();
        private readonly Queue<(Type type, object data)> pendingTransitions = new();

        private bool running;

        public void Transition<T1>() where T1 : T
        {
            Transition<T1, object>(default);
        }

        public void Transition<T1, T2>(T2 data) where T1 : T
        {
            var type = typeof(T1);

            if (!states.ContainsKey(type))
            {
                var state = (T)container.Resolve(type);
                states.Add(type, state);
            }

            pendingTransitions.Enqueue((type, data));

            StartIfShould();
        }

        private void StartIfShould()
        {
            if (running)
                return;

            running = true;

            Update();
        }

        public void Stop()
        {
            running = false;

            pendingTransitions.Clear();
        }

        private async void Update()
        {
            while (pendingTransitions.Count > 0)
            {
                if (!running)
                    break;

                var transition = pendingTransitions.Dequeue();
                var cancel = await ChangeTo(transition.type, transition.data).SuppressCancellationThrow();
                if (cancel)
                    running = false;
            }

            running = false;
        }

        private async UniTask ChangeTo(Type type, object data)
        {
            if (CurrentState != null)
            {
                var previousState = CurrentState;

                Debug.Log($"Change state from '{CurrentState.GetType()}'.");

                await previousState.OnExit();

                CurrentState = null;
            }

            var success = states.TryGetValue(type, out T nextState);
            Assert.IsTrue(success, $"State '{type.Name}' is not registered to state machine.");

            nextState.Data = data;

            CurrentState = nextState;

            Debug.Log($"Change state to '{CurrentState.GetType()}'.");

            await CurrentState.OnEnter();
        }
    }
}
