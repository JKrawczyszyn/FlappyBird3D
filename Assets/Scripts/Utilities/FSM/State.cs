using Cysharp.Threading.Tasks;

namespace Utilities.FSM
{
    public interface IState
    {
        object Data { set; }

        UniTask OnEnter();
    }

    public abstract class State : IState
    {
        public object Data { protected get; set; }

        public virtual async UniTask OnEnter()
        {
            await UniTask.Yield();
        }
    }
}
