using Cysharp.Threading.Tasks;
using Zenject;
using NUnit.Framework;
using Utilities.FSM;

[TestFixture]
public class StateMachineTest : ZenjectUnitTestFixture
{
    private StateMachine<State> stateMachine;
    private DummyState1 state1;
    private DummyState2 state2;

    [SetUp]
    public void SetUp()
    {
        Container.Bind<StateMachine<State>>().AsSingle();
        Container.Bind<DummyState1>().AsSingle();
        Container.Bind<DummyState2>().AsSingle();

        stateMachine = Container.Resolve<StateMachine<State>>();
        state1 = Container.Resolve<DummyState1>();
        state2 = Container.Resolve<DummyState2>();
    }

    [Test]
    public void Transition_NewState_StateIsRegistered()
    {
        stateMachine.Transition<DummyState1>();

        Assert.That(stateMachine.CurrentState, Is.EqualTo(state1));
    }

    [Test]
    public void Stop_StateMachine_StopsAndClearsQueue()
    {
        stateMachine.Transition<DummyState1>();
        stateMachine.Stop();

        Assert.That(stateMachine.CurrentState, Is.Null);
    }

    [Test]
    public void Transition_NewState_StateChangesCorrectly()
    {
        stateMachine.Transition<DummyState1>();
        stateMachine.Transition<DummyState2>();

        Assert.That(stateMachine.CurrentState, Is.EqualTo(state2));
    }

    [Test]
    public void Transition_PreviousState_StateChangesCorrectly()
    {
        stateMachine.Transition<DummyState1>();
        stateMachine.Transition<DummyState2>();
        stateMachine.Transition<DummyState1>();

        Assert.That(stateMachine.CurrentState, Is.EqualTo(state1));
    }

    [Test]
    public void Transition_SameState_NoStateChange()
    {
        stateMachine.Transition<DummyState1>();
        stateMachine.Transition<DummyState1>();

        Assert.That(stateMachine.CurrentState, Is.EqualTo(state1));
    }

    [Test]
    public void Execution_RepeatedState_AllStatesExecutedCorrectNumberOfTimes()
    {
        stateMachine.Transition<DummyState2>();
        stateMachine.Transition<DummyState1>();
        stateMachine.Transition<DummyState2>();

        Assert.That(state1.Executed, Is.EqualTo(1));
        Assert.That(state2.Executed, Is.EqualTo(2));
    }

    private class DummyBaseState : State
    {
        public int Executed;

        public override async UniTask OnEnter()
        {
            Executed++;
        }
    }

    private class DummyState1 : DummyBaseState
    {
    }

    private class DummyState2 : DummyBaseState
    {
    }
}
