using System;
using System.Collections.Generic;
using Zenject;

namespace Utilities
{
    public class StateMachine<T> : IDisposable where T : IState
    {
        [Inject]
        private readonly DiContainer container;

        private readonly Stack<T> states = new();

        private T CurrentState => states.Peek();

        public void Push<T2>() where T2 : T
        {
            var state = container.Resolve<T2>();
            states.Push(state);
        }

        public void Pop()
        {
            states.Pop();
        }

        public void Perform()
        {
            CurrentState.Perform();
        }

        public void Dispose()
        {
            states.Clear();
        }
    }
}
