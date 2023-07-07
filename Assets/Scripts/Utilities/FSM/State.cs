using Cysharp.Threading.Tasks;

namespace Utilities.FSM
{
    public interface IState
    {
        object Data { set; }

        UniTask OnEnter();
        UniTask OnExit();
    }

    public abstract class State : IState
    {
        public object Data { protected get; set; }

        public virtual async UniTask OnEnter()
        {
            await UniTask.Yield();
        }

        public virtual async UniTask OnExit()
        {
            await UniTask.Yield();
        }
    }
}
