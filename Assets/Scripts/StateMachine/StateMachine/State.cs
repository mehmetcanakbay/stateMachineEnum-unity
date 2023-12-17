using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MAKStateMachine {
    public abstract class State<T> where T : IBaseStateData
    {
        protected T stateData;
        
        public abstract void Tick();
        public abstract void OnTransitionEnter();
        public abstract void OnTransitionExit();
        public abstract void OnInitialize();
        public virtual void Initialize(T stateDataToInitializeWith) {
            stateData = stateDataToInitializeWith;
            OnInitialize();
        }
    }
}
