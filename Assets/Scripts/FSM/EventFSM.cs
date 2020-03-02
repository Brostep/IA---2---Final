using System;
using UnityEngine;

namespace InteligenciaArtificial2 {
    public class EventFSM<T> {
        public State<T> _any;

        public State<T> Current { get; set; }

        public EventFSM(State<T> initial, State<T> any = null) {
            Current = initial;
            Current.Enter(default(T));
            _any = any != null ? any : new State<T>("<any>");
            _any.OnEnter += a => { throw new Exception("Was not able to transition to the <any> state."); };
        }

        public bool Feed(T input) {
            State<T> newState;
            
            if (Current.Feed(input, out newState) || _any.Feed(input, out newState)) {
                Current.Exit(input);
                Debug.Log("FSM state: " + Current.Name + "---" + input + "---> " + newState.Name);
                Current = newState;
                Current.Enter(input);

                return true;
            }

            return false;
        }
        
        public void Update() {
            Current.Update();
        }
    }
}
