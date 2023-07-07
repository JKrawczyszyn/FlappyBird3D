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

        private T currentState;
        private T previousState;

        private readonly Dictionary<Type, T> states = new();
        private readonly Queue<Type> pendingTransitions = new();

        private bool running;

        public void RequestTransition<T1>() where T1 : T
        {
            RequestTransition(typeof(T1));
        }

        public void RequestTransition(Type type)
        {
            if (!states.ContainsKey(type))
            {
                var state = (T)container.Resolve(type);
                states.Add(type, state);
            }

            pendingTransitions.Enqueue(type);
        }

        public void Start()
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
                var cancel = await ChangeTo(transition).SuppressCancellationThrow();
                if (cancel)
                    running = false;
            }

            running = false;
        }

        private async UniTask ChangeTo(Type stateType)
        {
            if (currentState != null)
            {
                previousState = currentState;

                Debug.Log($"Change state from '{currentState.GetType()}'.");

                await previousState.OnExit();

                currentState = null;
            }

            var success = states.TryGetValue(stateType, out T nextState);
            Assert.IsTrue(success, $"State '{stateType.Name}' is not registered to state machine.");

            currentState = nextState;

            Debug.Log($"Change state to '{currentState.GetType()}'.");

            await nextState.OnEnter();
        }
    }
}
