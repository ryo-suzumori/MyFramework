using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniRx;
using Zenject;

namespace MyFw
{
    public interface IStateBase<OwnerClass>
    {
        void OnEnter();
        void OnExit();
        void OnDispose();
    }

    public interface IObservableState
    {
        IObservable<Unit> OnEnterAsObservable { get; }
        IObservable<Unit> OnExitAsObservable { get; }
        void RequestNext();
        void RequestNext(int stateId);
    }

    public class StateBase<OwnerClass> : IStateBase<OwnerClass>
        where OwnerClass : StateMachineV2<OwnerClass>
    {
        [Inject] protected readonly OwnerClass ownerClass;
        protected readonly CompositeDisposable disposables = new();
        
        public virtual void OnEnter() {}
        public virtual void OnExit() {}

        public void OnDispose()
        {
            this.disposables.Clear();
        }
    }

    public abstract class ObservableState<OwnerClass> : StateBase<OwnerClass>, IObservableState
        where OwnerClass : StateMachineV2<OwnerClass>
    {
        public virtual TypeInfo NextState { get; }

        private readonly Subject<Unit> onEnter = new();
        public IObservable<Unit> OnEnterAsObservable => this.onEnter;
        private readonly Subject<Unit> onExit = new();
        public IObservable<Unit> OnExitAsObservable => this.onExit;
        
        public override void OnEnter() => this.onEnter.OnNext(Unit.Default);
        public override void OnExit() => this.onExit.OnNext(Unit.Default);
        public void RequestNext() => this.ownerClass.SwitchState(NextState);
        public virtual void RequestNext(int stateId) { }
    }

    public class StateMachineV2<OwnerClass> : ITickable, IInitializable
        where OwnerClass : StateMachineV2<OwnerClass>
    {
        [Inject] private readonly IEnumerable<IStateBase<OwnerClass>> states;
        private IStateBase<OwnerClass> currentState;
        private TypeInfo reserveState;

        public void Initialize()
        {
            this.reserveState = this.states.FirstOrDefault().GetType().GetTypeInfo();
        }

        public void Tick()
        {
            if (this.reserveState == null)
            {
                return;
            }

            var nextState = this.states.FirstOrDefault(state => state.GetType() == this.reserveState);
            if (nextState == null)
            {
                throw new ApplicationException($"this state is not register {this.reserveState.Name}");
            }
            // OnEnterで変更できるように先に初期化する.
            this.reserveState = null;

#if UNITY_EDITOR
            var currenName = this.currentState != null
                ? this.currentState.GetType().Name
                : "noneState";
            var nextName = nextState.GetType().Name;
            LogUtil.Log($"{typeof(OwnerClass).Name}:{currenName} => {nextName}");
#endif
            // 各ステートの更新.
            this.currentState?.OnExit();
            this.currentState?.OnDispose();
            this.currentState = nextState;
            this.currentState.OnEnter();
        }

        public void SwitchState<StateClass>()
            where StateClass : StateBase<OwnerClass>
        {
            this.reserveState = typeof(StateClass).GetTypeInfo();
        }

        public void SwitchState(TypeInfo typeInfo)
        {
            if (typeInfo == null)
            {
                LogUtil.LogWarning($"Switch state is null");
                return;
            }
            this.reserveState = typeInfo;
        }

        public bool CompareState<StateClass>()
            => currentState.GetType() == typeof(StateClass);
    }
}