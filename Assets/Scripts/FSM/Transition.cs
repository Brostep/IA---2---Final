using System;

namespace InteligenciaArtificial2 {
	public class Transition<T> {
        public T Input { get; set; }
        public State<T> TargetState { get; set; }
        public event Action<T> OnTransition = delegate { };
        
        public Transition(T input, State<T> targetState) {
            Input = input;
            TargetState = targetState;
        }

        public void OnTransitionExecute(T input) {
			OnTransition(input);
		}
	}
}