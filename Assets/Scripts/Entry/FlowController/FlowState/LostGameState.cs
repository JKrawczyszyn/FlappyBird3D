using Cysharp.Threading.Tasks;

namespace Entry.Controllers
{
    public class LostGameState : FlowState
    {
        public override async UniTask OnEnter()
        {
            await UniTask.Yield();

            StateMachine.RequestTransition(typeof(LoadMenuState));
        }
    }
}
