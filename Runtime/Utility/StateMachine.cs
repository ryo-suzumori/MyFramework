using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniRx;
using UnityEngine;

namespace MyFw
{
    public interface IState<in OwnerClass>
        where OwnerClass : StateMachine<OwnerClass>
    {
        void OnEnter(OwnerClass owner);
        void OnExit(OwnerClass owner);
    }

    public class StateMachine<OwnerClass>
        where OwnerClass : StateMachine<OwnerClass>
    {
        private readonly List<IState<OwnerClass>> stateList = new();
        private IState<OwnerClass> currentState;
        private TypeInfo reserveState;

        public StateMachine(IEnumerable<IState<OwnerClass>> stateList)
        {
            this.stateList.AddRange(stateList);
        }

        public void Update()
        {
            if (this.reserveState == null)
            {
                return;
            }

            var nextState = this.stateList.FirstOrDefault(state => state.GetType() == this.reserveState);
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
            Debug.Log($"{typeof(OwnerClass).Name}:{currenName} => {nextName}");
#endif
            // 各ステートの更新.
            this.currentState?.OnExit(this as OwnerClass);
            this.currentState = nextState;
            this.currentState.OnEnter(this as OwnerClass);
        }

        public void SwitchState<StateClass>() where StateClass : class, IState<OwnerClass>
        {
            this.reserveState = typeof(StateClass).GetTypeInfo();
        }

        public bool CompareState<StateClass>()
            => currentState.GetType() == typeof(StateClass);
    }
}